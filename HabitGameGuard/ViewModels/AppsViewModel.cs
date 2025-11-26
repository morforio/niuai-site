using System.Collections.ObjectModel;
using HabitGameGuard.Models;
using HabitGameGuard.Services;

namespace HabitGameGuard.ViewModels;

public class AppsViewModel : BaseViewModel
{
    private readonly AplicativoBloqueadoService _service;
    public ObservableCollection<AplicativoBloqueado> Apps { get; } = new();

    private AplicativoBloqueado? _selected;
    public AplicativoBloqueado? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public AppsViewModel(AplicativoBloqueadoService service)
    {
        _service = service;
        Carregar();
    }

    public void Carregar()
    {
        Apps.Clear();
        foreach (var app in _service.Listar())
        {
            Apps.Add(app);
        }
    }

    public void Salvar(AplicativoBloqueado app)
    {
        _service.Salvar(app);
        Carregar();
    }

    public void Remover(AplicativoBloqueado app)
    {
        _service.Remover(app);
        Carregar();
    }
}
