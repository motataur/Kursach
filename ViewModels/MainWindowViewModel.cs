using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kursach.DB;
using Kursach.Models;
using Kursach.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Kursach.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ClientRepository _repository;
    private readonly RoomRepository _roomRepository;
    private readonly ReservationRepository _reservationRepository;
    private readonly RentRepository _rentRepository;
    private List<Client> _allClients = new();
    private List<Room> _allRooms = new();
    private List<Reservation> _allReservations = new();
    [ObservableProperty] private string _checkInRoomNumber;
    [ObservableProperty] private string _roomStatusMessage;
    [ObservableProperty] private bool _isRoomAvailable;
    [ObservableProperty] private string _clientSearchText;
    [ObservableProperty] private ObservableCollection<Client> _clientSuggestions = new();
    [ObservableProperty] private Client? _selectedClient;
    
    [ObservableProperty] private ObservableCollection<Reservation> _filteredReservations = new();
    [ObservableProperty] private string _reservationSearchText;

    [ObservableProperty] private ObservableCollection<Client> _filteredClients = new();
    [ObservableProperty] private ObservableCollection<Room> _filteredRooms = new();
    
    [ObservableProperty] private string _searchText;
    [ObservableProperty] private int _selectedSortIndex = 0;
    
    [ObservableProperty] private string _roomSearchText;
    [ObservableProperty] private int _selectedRoomStatusIndex = 0;
    private int? _reservedByClientId = null;

    public MainWindowViewModel(IServiceProvider serviceProvider, ClientRepository repository, 
        RoomRepository roomRepository, ReservationRepository reservationRepository, RentRepository rentRepository)
    {
        _serviceProvider = serviceProvider;
        _repository = repository;
        _roomRepository = roomRepository;
        _reservationRepository = reservationRepository;
        LoadClients();
        LoadRooms();
        LoadReservations();
        _rentRepository = rentRepository;
    }

    partial void OnSearchTextChanged(string value) => ApplyFilterAndSort();
    partial void OnSelectedSortIndexChanged(int value) => ApplyFilterAndSort();
    partial void OnRoomSearchTextChanged(string value) => ApplyRoomFilterAndSort();
    partial void OnSelectedRoomStatusIndexChanged(int value) => ApplyRoomFilterAndSort();
    partial void OnReservationSearchTextChanged(string value) => ApplyReservationFilter();
    
    private void ApplyReservationFilter()
    {
        if (_allReservations == null) return;
        var result = _allReservations.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(ReservationSearchText))
        {
            var query = ReservationSearchText.ToLower().Trim();
            result = result.Where(r =>
                r.RoomNumber.ToString().Contains(query) ||
                (r.ClientLastName != null && r.ClientLastName.ToLower().Contains(query))
            );
        }

        FilteredReservations = new ObservableCollection<Reservation>(result);
    }

    public void LoadReservations()
    {
        _allReservations = _reservationRepository.GetReservationList();
        ApplyReservationFilter();
    }

    private void ApplyFilterAndSort()
    {
        if (_allClients == null) return;
        var result = _allClients.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var query = SearchText.ToLower().Trim();
            result = result.Where(c => 
                (c.LastName != null && c.LastName.ToLower().Contains(query)) ||
                (c.Name != null && c.Name.ToLower().Contains(query)) ||
                (c.Phone != null && c.Phone.Contains(query))
            );
        }

        result = SelectedSortIndex switch
        {
            0 => result.OrderBy(c => c.LastName),         
            1 => result.OrderByDescending(c => c.LastName), 
            2 => result.OrderByDescending(c => c.Id),       
            _ => result
        };

        FilteredClients = new ObservableCollection<Client>(result);
    }

    private void ApplyRoomFilterAndSort()
    {
        if (_allRooms == null) return;
        var result = _allRooms.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(RoomSearchText))
        {
            var query = RoomSearchText.ToLower().Trim();
            result = result.Where(r =>
                r.Number.ToString().Contains(query) ||
                (r.Name != null && r.Name.ToLower().Contains(query))
            );
        }

        var reservedNumbers = _reservationRepository.GetReservationList()
            .Select(r => r.RoomNumber)
            .ToHashSet();

        result = SelectedRoomStatusIndex switch
        {
            1 => result.Where(r => r.IsFree),
            2 => result.Where(r => !r.IsFree),
            3 => result.Where(r => reservedNumbers.Contains(r.Number)),
            _ => result
        };

        FilteredRooms = new ObservableCollection<Room>(result);
    }
    partial void OnCheckInRoomNumberChanged(string value)
    {
        RoomStatusMessage = string.Empty;
        IsRoomAvailable = false;
        _reservedByClientId = null;
        SelectedClient = null;
        ClientSearchText = string.Empty;

        if (int.TryParse(value, out int num))
        {
            var room = _roomRepository.GetRoomByNumber(num);
            if (room == null)
            {
                RoomStatusMessage = "Комната не найдена";
                return;
            }
            if (!room.IsFree)
            {
                RoomStatusMessage = "❌ Комната занята";
                return;
            }

            var reservation = _allReservations.FirstOrDefault(r => r.RoomNumber == num);
            if (reservation != null)
            {
                _reservedByClientId = reservation.ClientId;
                RoomStatusMessage = "⚠️ Комната забронирована — клиент подставлен автоматически";

                // Автоподстановка клиента
                var client = _allClients.FirstOrDefault(c => c.Id == reservation.ClientId);
                if (client != null)
                {
                    SelectedClient = client;
                    ClientSearchText = client.LastName ?? string.Empty;
                }
            }
            else
            {
                RoomStatusMessage = "✅ Комната свободна";
            }

            IsRoomAvailable = true;
        }
    }

    partial void OnClientSearchTextChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            ClientSuggestions.Clear();
            return;
        }
        var query = value.ToLower();
        var filtered = _allClients
            .Where(c => c.LastName != null && c.LastName.ToLower().Contains(query))
            .Take(5)
            .ToList();
        ClientSuggestions = new ObservableCollection<Client>(filtered);
    }

    [RelayCommand]
    public async Task OpenConfirmRentWindowAsync()
    {
        if (!int.TryParse(CheckInRoomNumber, out int roomNum)) return;
        var room = _roomRepository.GetRoomByNumber(roomNum);
        if (room == null || !room.IsFree || SelectedClient == null) return;

        // Если комната забронирована — пускаем только того кто бронировал
        if (_reservedByClientId.HasValue && SelectedClient.Id != _reservedByClientId.Value)
        {
            RoomStatusMessage = "❌ Комната забронирована другим клиентом";
            return;
        }

        var vm = new ConfirmRentWindowViewModel(
            SelectedClient, room, _rentRepository, _roomRepository, _reservationRepository,
            () => { LoadRooms(); LoadClients(); LoadReservations(); }
        );

        var window = new ConfirmRentWindow(vm);
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            await window.ShowDialog(desktop.MainWindow);
        }
    }

    public void LoadClients()
    {
        _allClients = _repository.GetClientList();
        ApplyFilterAndSort();
    }

    public void LoadRooms()
    {
        _roomRepository.FreeExpiredRooms();
        _allRooms = _roomRepository.GetRoomList();
        ApplyRoomFilterAndSort();
    }

    [RelayCommand]
    public async Task OpenAddClientWindowAsync()
    {
        var window = _serviceProvider.GetRequiredService<AddClientWindow>();
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            await window.ShowDialog(desktop.MainWindow);
            LoadClients();
        }
    }
    [RelayCommand]
    public async Task OpenAddReservationWindowAsync()
    {
        var vm = new AddReservationWindowViewModel(
            _reservationRepository, _roomRepository, _allClients, _allReservations,
            () => { LoadReservations(); LoadRooms(); }
        );
        var window = new AddReservationWindow(vm);
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            await window.ShowDialog(desktop.MainWindow);
        }
    }
}