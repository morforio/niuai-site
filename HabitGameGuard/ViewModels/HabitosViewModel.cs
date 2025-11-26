using System.Collections.ObjectModel;
using HabitGameGuard.Models;
using HabitGameGuard.Services;

namespace HabitGameGuard.ViewModels;

public class HabitosViewModel : BaseViewModel
{
    private readonly HabitoService _habitoService;
    public ObservableCollection<Habit> Habitos { get; } = new();

    private Habit? _selectedHabit;
    public Habit? SelectedHabit
    {
        get => _selectedHabit;
        set => SetProperty(ref _selectedHabit, value);
    }

    public HabitosViewModel(HabitoService habitoService)
    {
        _habitoService = habitoService;
        Carregar();
    }

    public void Carregar()
    {
        Habitos.Clear();
        foreach (var h in _habitoService.ListarTodos())
        {
            Habitos.Add(h);
        }
    }

    public void Salvar(Habit habit)
    {
        _habitoService.Salvar(habit);
        Carregar();
    }

    public void Remover(Habit habit)
    {
        _habitoService.Remover(habit);
        Carregar();
    }

    public void MarcarConcluido(Habit habit)
    {
        _habitoService.RegistrarExecucao(habit.Id, DateOnly.FromDateTime(DateTime.Now));
        Carregar();
    }
}
