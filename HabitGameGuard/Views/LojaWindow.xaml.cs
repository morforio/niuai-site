using System.Windows;
using HabitGameGuard.ViewModels;

namespace HabitGameGuard.Views;

public partial class LojaWindow : Window
{
    private readonly LojaViewModel _viewModel;

    public LojaWindow(LojaViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    private void Comprar_Click(object sender, RoutedEventArgs e)
    {
        var compradas = _viewModel.Comprar();
        MessageBox.Show(compradas > 0 ? $"Partidas liberadas: {compradas}" : "Créditos insuficientes ou valor inválido.");
    }

    private void Fechar_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
