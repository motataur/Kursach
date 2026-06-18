using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kursach.DB;
using Kursach.Models;

namespace Kursach.ViewModels;

public partial class AddReservationWindowViewModel : ViewModelBase
{
    private readonly ReservationRepository _reservationRepository;
    private readonly RoomRepository _roomRepository;
    private readonly List<Client> _allClients;
    private readonly Action _onSuccess;

    [ObservableProperty] private string _roomNumber;
    [ObservableProperty] private string _roomStatusMessage;
    [ObservableProperty] private string _clientSearchText;
    [ObservableProperty] private ObservableCollection<Client> _clientSuggestions = new();
    [ObservableProperty] private Client? _selectedClient;
    [ObservableProperty] private DateTimeOffset _reservationDate = DateTimeOffset.Now;

    private readonly List<Reservation> _allReservations;

    public AddReservationWindowViewModel(ReservationRepository reservationRepository,
        RoomRepository roomRepository, List<Client> allClients, 
        List<Reservation> allReservations, Action onSuccess)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
        _allClients = allClients;
        _allReservations = allReservations;
        _onSuccess = onSuccess;
    }

    partial void OnRoomNumberChanged(string value)
    {
        RoomStatusMessage = string.Empty;
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

            // Проверяем есть ли уже бронь
            var alreadyReserved = _allReservations.Any(r => r.RoomNumber == num);
            if (alreadyReserved)
            {
                RoomStatusMessage = "❌ Комната уже забронирована";
                return;
            }

            RoomStatusMessage = "✅ Комната свободна";
        }
    }

    partial void OnClientSearchTextChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) { ClientSuggestions.Clear(); return; }
        var query = value.ToLower();
        ClientSuggestions = new ObservableCollection<Client>(
            _allClients.Where(c => c.LastName != null && c.LastName.ToLower().Contains(query)).Take(5)
        );
    }

    [RelayCommand]
    public void Save(object? parameter)
    {
        if (!int.TryParse(RoomNumber, out int roomNum)) return;
        if (SelectedClient == null) return;

        var room = _roomRepository.GetRoomByNumber(roomNum);
        if (room == null || !room.IsFree) return;

        _reservationRepository.AddReservation(new Reservation
        {
            RoomNumber = roomNum,
            ClientId = SelectedClient.Id,
            ReservationDate = ReservationDate.DateTime
        });

        _onSuccess?.Invoke();

        if (parameter is Window window)
            window.Close();
    }

    [RelayCommand]
    public void Cancel(object? parameter)
    {
        if (parameter is Window window)
            window.Close();
    }
}