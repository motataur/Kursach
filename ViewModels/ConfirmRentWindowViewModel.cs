using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kursach.DB;
using Kursach.Models;

namespace Kursach.ViewModels;

public partial class ConfirmRentWindowViewModel : ViewModelBase
{
    private readonly RentRepository _rentRepository;
    private readonly RoomRepository _roomRepository;
    private readonly ReservationRepository _reservationRepository;
    private readonly Action _onSuccess;

    public Client Client { get; }
    public Room Room { get; }

    [ObservableProperty] private DateTimeOffset _startDate = DateTimeOffset.Now;
    [ObservableProperty] private DateTimeOffset _endDate = DateTimeOffset.Now.AddDays(1);
    [ObservableProperty] private int _totalPrice;

    public string ClientInfo => $"{Client.LastName} {Client.Name}";
    public string RoomInfo => $"№{Room.Number} — {Room.Name}, {Room.Area} м²";
    public string Phone => Client.Phone ?? "";

    private void RecalcPrice()
    {
        var days = Math.Max(1, (EndDate.Date - StartDate.Date).Days);
        TotalPrice = Room.PricePerN * days;
    }

    partial void OnStartDateChanged(DateTimeOffset value) => RecalcPrice();
    partial void OnEndDateChanged(DateTimeOffset value) => RecalcPrice();

    public ConfirmRentWindowViewModel(Client client, Room room,
        RentRepository rentRepository, RoomRepository roomRepository,
        ReservationRepository reservationRepository, Action onSuccess)
    {
        Client = client;
        Room = room;
        _rentRepository = rentRepository;
        _roomRepository = roomRepository;
        _reservationRepository = reservationRepository;
        _onSuccess = onSuccess;
        RecalcPrice();
    }

    [RelayCommand]
    public void Confirm(object? parameter)
    {
        var rent = new Rent
        {
            RoomNumber = Room.Number,
            ClientId = Client.Id,
            StartDate = StartDate.DateTime,
            EndDate = EndDate.DateTime
        };

        Console.WriteLine($"Добавляем аренду: комната {rent.RoomNumber}, клиент {rent.ClientId}");
        _rentRepository.AddRent(rent);
    
        Console.WriteLine($"Меняем статус комнаты {Room.Number} на занятую (false/0)");
        _roomRepository.SetRoomFree(Room.Number, false);
    
        Console.WriteLine($"Удаляем бронь для комнаты {Room.Number}");
        _reservationRepository.DeleteReservationByRoom(Room.Number);
        Console.WriteLine($"Вызываем SetRoomFree для комнаты {Room.Number}");
        _roomRepository.SetRoomFree(Room.Number, false);
    
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