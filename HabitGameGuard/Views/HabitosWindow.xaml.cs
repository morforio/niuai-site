using System.Windows;
using HabitGameGuard.Models;
using HabitGameGuard.ViewModels;

namespace HabitGameGuard.Views;

public partial class HabitosWindow : Window
{
    private readonly HabitosViewModel _viewModel;

    public HabitosWindow(HabitosViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    private void Adicionar_Click(object sender, RoutedEventArgs e)
    {
        var habit = new Habit
        {
            Nome = "Novo hábito",
            Descricao = "Descreva aqui",
            MetaDiaria = 1,
            Dificuldade = Dificuldade.Facil,
            Ativo = true
        };
        _viewModel.Salvar(habit);
    }

    private void Editar_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.SelectedHabit is null)
        {
            return;
        }
        _viewModel.Salvar(_viewModel.SelectedHabit);
    }

    private void Remover_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.SelectedHabit is null)
        {
            return;
        }
        _viewModel.Remover(_viewModel.SelectedHabit);
    }

    private void Concluir_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.SelectedHabit is null)
        {
            return;
        }
        _viewModel.MarcarConcluido(_viewModel.SelectedHabit);
    }
}
