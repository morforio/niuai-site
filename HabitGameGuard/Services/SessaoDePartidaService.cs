using HabitGameGuard.Data;
using HabitGameGuard.Models;

namespace HabitGameGuard.Services;

public class SessaoDePartidaService
{
    private readonly AppDbContext _db;
    private readonly CreditoService _creditoService;

    public SessaoDePartidaService(AppDbContext db, CreditoService creditoService)
    {
        _db = db;
        _creditoService = creditoService;
        _db.Database.EnsureCreated();
    }

    public SessaoDePartida ObterOuCriarSessao(int aplicativoId)
    {
        var sessao = _db.Sessoes.FirstOrDefault(s => s.AplicativoBloqueadoId == aplicativoId);
        if (sessao is null)
        {
            sessao = new SessaoDePartida
            {
                AplicativoBloqueadoId = aplicativoId,
                PartidasLiberadas = 0,
                PartidasConsumidas = 0,
                PartidaEmAndamento = false
            };
            _db.Sessoes.Add(sessao);
            _db.SaveChanges();
        }
        return sessao;
    }

    public int CalcularPartidasDisponiveis(SessaoDePartida sessao) => sessao.PartidasLiberadas - sessao.PartidasConsumidas;

    public int ComprarPartidas(AplicativoBloqueado app, int creditosASeremGastos)
    {
        if (creditosASeremGastos <= 0)
        {
            return 0;
        }

        var partidas = creditosASeremGastos / app.CreditosPorPartida;
        if (partidas <= 0)
        {
            return 0;
        }

        var debitoOk = _creditoService.DebitarCreditos(creditosASeremGastos);
        if (!debitoOk)
        {
            return 0;
        }

        var sessao = ObterOuCriarSessao(app.Id);
        sessao.PartidasLiberadas += partidas;
        _db.SaveChanges();
        return partidas;
    }

    public void IniciarPartida(AplicativoBloqueado app, string gameId)
    {
        var sessao = ObterOuCriarSessao(app.Id);
        sessao.PartidaEmAndamento = true;
        sessao.GameIdAtual = gameId;
        sessao.HoraInicioPartidaAtual = DateTime.Now;
        _db.SaveChanges();
    }

    public bool FinalizarPartida(AplicativoBloqueado app)
    {
        var sessao = ObterOuCriarSessao(app.Id);
        if (sessao.GameIdAtual is null)
        {
            return false;
        }

        if (sessao.GameIdAtual != sessao.UltimoGameIdDebitado)
        {
            if (CalcularPartidasDisponiveis(sessao) > 0)
            {
                sessao.PartidasConsumidas++;
            }
            sessao.UltimoGameIdDebitado = sessao.GameIdAtual;
        }

        sessao.PartidaEmAndamento = false;
        sessao.HoraFimPartidaAtual = DateTime.Now;
        sessao.GameIdAtual = null;
        sessao.HoraInicioPartidaAtual = null;
        _db.SaveChanges();
        return true;
    }
}
