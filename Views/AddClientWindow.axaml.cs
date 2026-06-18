using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kursach.ViewModels;

namespace Kursach.Views;

public partial class AddClientWindow : Window
{
    public AddClientWindow(AddClientWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
}