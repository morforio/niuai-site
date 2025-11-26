using System.Collections.ObjectModel;
using HabitGameGuard.Models;
using HabitGameGuard.Services;

namespace HabitGameGuard.ViewModels;

public class LojaViewModel : BaseViewModel
{
    private readonly AplicativoBloqueadoService _appService;
    private readonly SessaoDePartidaService _sessaoService;
    private readonly CreditoService _creditoService;

    public ObservableCollection<AplicativoBloqueado> Apps { get; } = new();

    private AplicativoBloqueado? _appSelecionado;
    public AplicativoBloqueado? AppSelecionado
    {
        get => _appSelecionado;
        set
        {
            if (SetProperty(ref _appSelecionado, value))
            {
                AtualizarSessao();
            }
        }
    }

    private SessaoDePartida? _sessao;
    public SessaoDePartida? Sessao
    {
        get => _sessao;
        set
        {
            if (SetProperty(ref _sessao, value))
            {
                OnPropertyChanged(nameof(PartidasDisponiveis));
            }
        }
    }

    private int _saldo;
    public int Saldo
    {
        get => _saldo;
        set => SetProperty(ref _saldo, value);
    }

    private int _creditosCompra;
    public int CreditosCompra
    {
        get => _creditosCompra;
        set => SetProperty(ref _creditosCompra, value);
    }

    public LojaViewModel(AplicativoBloqueadoService appService, SessaoDePartidaService sessaoService, CreditoService creditoService)
    {
        _appService = appService;
        _sessaoService = sessaoService;
        _creditoService = creditoService;
        Carregar();
    }

    public void Carregar()
    {
        Apps.Clear();
        foreach (var app in _appService.Listar())
        {
            Apps.Add(app);
        }
        AppSelecionado = Apps.FirstOrDefault();
        Saldo = _creditoService.ObterSaldo();
    }

    private void AtualizarSessao()
    {
        if (AppSelecionado is null)
        {
            Sessao = null;
            return;
        }
        Sessao = _sessaoService.ObterOuCriarSessao(AppSelecionado.Id);
    }

    public int PartidasDisponiveis => Sessao is null ? 0 : _sessaoService.CalcularPartidasDisponiveis(Sessao);

    public int Comprar()
    {
        if (AppSelecionado is null)
        {
            return 0;
        }
        var compradas = _sessaoService.ComprarPartidas(AppSelecionado, CreditosCompra);
        AtualizarSessao();
        Saldo = _creditoService.ObterSaldo();
        OnPropertyChanged(nameof(PartidasDisponiveis));
        return compradas;
    }
}
