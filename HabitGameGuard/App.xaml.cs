using System.Windows;
using System.Windows.Forms;
using HabitGameGuard.Data;
using HabitGameGuard.Services;
using HabitGameGuard.ViewModels;
using HabitGameGuard.Views;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace HabitGameGuard;

public partial class App : Application
{
    private NotifyIcon? _notifyIcon;
    private MonitorDeProcessosService? _monitor;
    private AppDbContext? _db;
    private CreditoService? _credito;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _db = new AppDbContext();
        _db.Database.EnsureCreated();

        _credito = new CreditoService(_db);
        var habito = new HabitoService(_db, _credito);
        var appService = new AplicativoBloqueadoService(_db);
        var sessaoService = new SessaoDePartidaService(_db, _credito);
        _monitor = new MonitorDeProcessosService(appService, sessaoService, _credito, _db);

        var viewModel = new DashboardViewModel(habito, _credito, appService, sessaoService);
        var mainWindow = new MainWindow(viewModel, habito, appService, sessaoService, _credito);
        mainWindow.Show();
        ConfigurarTray(mainWindow);
    }

    private void ConfigurarTray(Window mainWindow)
    {
        _notifyIcon = new NotifyIcon
        {
            Visible = true,
            Text = "HabitGameGuard",
            Icon = System.Drawing.SystemIcons.Application
        };

        var menu = new ContextMenuStrip();
        menu.Items.Add("Abrir", null, (_, _) => mainWindow.Show());
        menu.Items.Add("Pausar bloqueio (DEBUG)", null, (_, _) => ToggleDebug());
        menu.Items.Add("Sair", null, (_, _) => Encerrar());
        _notifyIcon.ContextMenuStrip = menu;
        _notifyIcon.DoubleClick += (_, _) => mainWindow.Show();
    }

    private void ToggleDebug()
    {
        if (_credito is null)
        {
            return;
        }
        var ativado = _credito.ToggleDebug();
        MessageBox.Show(ativado ? "Modo debug ativado: bloqueio pausado." : "Modo debug desativado.");
    }

    private void Encerrar()
    {
        _monitor?.Dispose();
        _notifyIcon?.Dispose();
        Current.Shutdown();
    }
}
