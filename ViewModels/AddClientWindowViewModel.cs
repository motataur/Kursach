using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kursach.DB;
using Kursach.Models;

namespace Kursach.ViewModels;

public partial class AddClientWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    public AddClientWindowViewModel (IServiceProvider serviceProvider, ClientRepository clientRepository)
    {
        _serviceProvider = serviceProvider;
        _clientRepository = clientRepository;
    }

    [ObservableProperty] public string _lastName;
    [ObservableProperty] public string _firstName;
    [ObservableProperty] public string _email;
    [ObservableProperty] public string _phone;
    [ObservableProperty] public string _passportSeries;
    [ObservableProperty] public string _passportNumber;
    private readonly ClientRepository _clientRepository;

    [RelayCommand]
    public void Cancel(object? parameter)
    {
      
        if (parameter is TopLevel topLevel)
        {
            (topLevel as Window)?.Close();
            return;
        }

        if (parameter is Window window)
        {
            window.Close();
        }
        
    }
    [RelayCommand]
    public void Save(object? parameter)
    {
        System.Diagnostics.Debug.WriteLine("Нажата кнопка Сохранить.");
        System.Diagnostics.Debug.WriteLine($"Значения: Name='{FirstName}', Lastname='{LastName}'");

        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            System.Diagnostics.Debug.WriteLine("Имя или Фамилия не заполнены!");
            return; 
        }

        try
        {
            var newClient = new Client
            {
                Name = FirstName,
                LastName = LastName, 
                Email = Email,
                Phone = Phone,
                PassportSeries = PassportSeries,
                PassportNumber = PassportNumber,
            };

            _clientRepository.AddClient(newClient);
            System.Diagnostics.Debug.WriteLine("Клиент успешно добавлен в базу данных!");

            if (parameter is Window window)
            {
                window.Close();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Окно не закрылось, потому что CommandParameter не передан или пустой.");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ОШИБКА");
        }
    }
}