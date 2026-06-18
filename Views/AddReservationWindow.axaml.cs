using Avalonia.Controls;
using Kursach.ViewModels;

namespace Kursach.Views;

public partial class AddReservationWindow : Window
{
    public AddReservationWindow()
    {
        InitializeComponent();
    }
    
    public AddReservationWindow(AddReservationWindowViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}