using System.Windows;
using HabitGameGuard.Services;
using HabitGameGuard.ViewModels;

namespace HabitGameGuard.Views;

public partial class MainWindow : Window
{
    private readonly DashboardViewModel _viewModel;
    private readonly HabitoService _habitoService;
    private readonly AplicativoBloqueadoService _appService;
    private readonly SessaoDePartidaService _sessaoService;
    private readonly CreditoService _creditoService;

    public MainWindow(DashboardViewModel viewModel, HabitoService habitoService, AplicativoBloqueadoService appService, SessaoDePartidaService sessaoService, CreditoService creditoService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _habitoService = habitoService;
        _appService = appService;
        _sessaoService = sessaoService;
        _creditoService = creditoService;
        DataContext = _viewModel;
    }

    private void HabitosBtn_Click(object sender, RoutedEventArgs e)
    {
        var vm = new HabitosViewModel(_habitoService);
        var janela = new HabitosWindow(vm);
        janela.ShowDialog();
        _viewModel.Carregar();
    }

    private void AppsBtn_Click(object sender, RoutedEventArgs e)
    {
        var vm = new AppsViewModel(_appService);
        var janela = new AppsWindow(vm);
        janela.ShowDialog();
        _viewModel.Carregar();
    }

    private void LojaBtn_Click(object sender, RoutedEventArgs e)
    {
        var vm = new LojaViewModel(_appService, _sessaoService, _creditoService);
        var janela = new LojaWindow(vm);
        janela.ShowDialog();
        _viewModel.Carregar();
    }
}
