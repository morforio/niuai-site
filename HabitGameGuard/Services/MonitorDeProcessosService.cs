using System.Diagnostics;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using HabitGameGuard.Models;
using HabitGameGuard.Data;

namespace HabitGameGuard.Services;

public class MonitorDeProcessosService : IDisposable
{
    private readonly AplicativoBloqueadoService _appService;
    private readonly SessaoDePartidaService _sessaoService;
    private readonly CreditoService _creditoService;
    private readonly AppDbContext _db;
    private readonly HttpClient _httpClient;
    private readonly Timer _timer;
    private readonly Dictionary<int, bool> _estadoEmPartida = new();
    private readonly Dictionary<int, string?> _gameIdMemoria = new();

    public MonitorDeProcessosService(AplicativoBloqueadoService appService, SessaoDePartidaService sessaoService, CreditoService creditoService, AppDbContext db)
    {
        _appService = appService;
        _sessaoService = sessaoService;
        _creditoService = creditoService;
        _db = db;

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = IgnoreCertificate
        };
        _httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(3)
        };

        var intervalo = _db.Configuracoes.First().IntervaloMonitoramentoSegundos;
        _timer = new Timer(async _ => await TickAsync(), null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(intervalo));
    }

    private bool IgnoreCertificate(HttpRequestMessage _, X509Certificate2? __, X509Chain? ___, SslPolicyErrors ____) => true;

    private async Task TickAsync()
    {
        var apps = _appService.Listar().Where(a => a.Ativo).ToList();
        foreach (var app in apps)
        {
            await ProcessarAppAsync(app);
        }
    }

    private async Task ProcessarAppAsync(AplicativoBloqueado app)
    {
        var processName = Path.GetFileNameWithoutExtension(app.NomeProcesso);
        var processos = Process.GetProcesses().Where(p => p.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase)).ToList();
        var sessao = _sessaoService.ObterOuCriarSessao(app.Id);
        var partidasDisponiveis = _sessaoService.CalcularPartidasDisponiveis(sessao);

        if (processos.Count == 0)
        {
            _estadoEmPartida[app.Id] = false;
            return;
        }

        if (partidasDisponiveis <= 0 && !_creditoService.DebugAtivo())
        {
            foreach (var proc in processos)
            {
                try
                {
                    proc.Kill();
                }
                catch
                {
                    // processo pode já ter sido fechado ou sem permissão; este app é auto-regulador, não anticheat.
                }
            }
            return;
        }

        var (emPartida, gameId) = await ConsultarPartidaLolAsync();
        var estavaEmPartida = _estadoEmPartida.ContainsKey(app.Id) && _estadoEmPartida[app.Id];

        if (emPartida && !estavaEmPartida)
        {
            _estadoEmPartida[app.Id] = true;
            _gameIdMemoria[app.Id] = gameId;
            if (!string.IsNullOrEmpty(gameId))
            {
                _sessaoService.IniciarPartida(app, gameId!);
            }
            return;
        }

        if (!emPartida && estavaEmPartida)
        {
            _estadoEmPartida[app.Id] = false;
            _sessaoService.FinalizarPartida(app);
            _gameIdMemoria[app.Id] = null;
        }
    }

    private async Task<(bool emPartida, string? gameId)> ConsultarPartidaLolAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://127.0.0.1:2999/liveclientdata/allgamedata");
            if (!response.IsSuccessStatusCode)
            {
                return (false, null);
            }
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
            {
                return (false, null);
            }

            using var doc = System.Text.Json.JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("gameData", out var gameData) && gameData.TryGetProperty("gameId", out var gameIdProp))
            {
                var gameId = gameIdProp.GetString();
                return (true, gameId);
            }
            return (true, null);
        }
        catch
        {
            return (false, null);
        }
    }

    public void Dispose()
    {
        _timer.Dispose();
        _httpClient.Dispose();
    }
}
