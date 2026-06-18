using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Kursach.ViewModels;
using Kursach.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Kursach;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // 1. Вытаскиваем ViewModel и Окно из DI-контейнера
            var vm = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            var win = _serviceProvider.GetRequiredService<MainWindow>();
        
            // 2. Связываем их друг с другом
            win.DataContext = vm;
        
            // 3. Назначаем это настроенное окно главным окном приложения
            desktop.MainWindow = win;
        }

        base.OnFrameworkInitializationCompleted();
    }
}