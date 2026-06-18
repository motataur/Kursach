using Avalonia.Controls;
using Kursach.ViewModels;

namespace Kursach.Views;

public partial class ConfirmRentWindow : Window
{
    public ConfirmRentWindow()
    {
        InitializeComponent();
    }
    
    public ConfirmRentWindow(ConfirmRentWindowViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}