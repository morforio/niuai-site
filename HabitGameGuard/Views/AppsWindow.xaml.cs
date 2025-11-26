using System.Windows;
using HabitGameGuard.Models;
using HabitGameGuard.ViewModels;

namespace HabitGameGuard.Views;

public partial class AppsWindow : Window
{
    private readonly AppsViewModel _viewModel;

    public AppsWindow(AppsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    private void Adicionar_Click(object sender, RoutedEventArgs e)
    {
        var app = new AplicativoBloqueado
        {
            NomeAmigavel = "League of Legends",
            NomeProcesso = "League of Legends.exe",
            CreditosPorPartida = 3,
            Ativo = true
        };
        _viewModel.Salvar(app);
    }

    private void Editar_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.Selected is null)
        {
            return;
        }
        _viewModel.Salvar(_viewModel.Selected);
    }

    private void Remover_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.Selected is null)
        {
            return;
        }
        _viewModel.Remover(_viewModel.Selected);
    }
}
