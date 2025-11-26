using System.Collections.ObjectModel;
using HabitGameGuard.Models;
using HabitGameGuard.Services;

namespace HabitGameGuard.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    private readonly HabitoService _habitoService;
    private readonly CreditoService _creditoService;
    private readonly AplicativoBloqueadoService _appService;
    private readonly SessaoDePartidaService _sessaoService;

    private int _saldo;
    public int Saldo
    {
        get => _saldo;
        set => SetProperty(ref _saldo, value);
    }

    public ObservableCollection<Habit> Habitos { get; } = new();
    public ObservableCollection<SessaoResumo> Sessoes { get; } = new();

    private int _habitosConcluidosHoje;
    public int HabitosConcluidosHoje
    {
        get => _habitosConcluidosHoje;
        set => SetProperty(ref _habitosConcluidosHoje, value);
    }

    public DashboardViewModel(HabitoService habitoService, CreditoService creditoService, AplicativoBloqueadoService appService, SessaoDePartidaService sessaoService)
    {
        _habitoService = habitoService;
        _creditoService = creditoService;
        _appService = appService;
        _sessaoService = sessaoService;
        Carregar();
    }

    public void Carregar()
    {
        Habitos.Clear();
        foreach (var h in _habitoService.ListarHabitosAtivos())
        {
            Habitos.Add(h);
        }

        Sessoes.Clear();
        foreach (var app in _appService.Listar())
        {
            var sessao = _sessaoService.ObterOuCriarSessao(app.Id);
            var disponiveis = _sessaoService.CalcularPartidasDisponiveis(sessao);
            Sessoes.Add(new SessaoResumo(app.NomeAmigavel, sessao.PartidasLiberadas, sessao.PartidasConsumidas, disponiveis));
        }

        Saldo = _creditoService.ObterSaldo();
        HabitosConcluidosHoje = _habitoService.CalcularHabitosConcluidosHoje();
    }
}

public record SessaoResumo(string NomeApp, int Liberadas, int Consumidas, int Disponiveis);
