using HabitGameGuard.Data;
using HabitGameGuard.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitGameGuard.Services;

public class CreditoService
{
    private readonly AppDbContext _db;

    public CreditoService(AppDbContext db)
    {
        _db = db;
        _db.Database.EnsureCreated();
    }

    public int ObterSaldo()
    {
        var settings = _db.Configuracoes.First();
        return settings.SaldoCreditos;
    }

    public void AdicionarCreditos(int quantidade)
    {
        var settings = _db.Configuracoes.First();
        settings.SaldoCreditos += quantidade;
        _db.SaveChanges();
    }

    public bool DebitarCreditos(int quantidade)
    {
        var settings = _db.Configuracoes.First();
        if (settings.SaldoCreditos < quantidade)
        {
            return false;
        }

        settings.SaldoCreditos -= quantidade;
        _db.SaveChanges();
        return true;
    }

    public bool ToggleDebug()
    {
        var settings = _db.Configuracoes.First();
        settings.DebugPausarBloqueio = !settings.DebugPausarBloqueio;
        _db.SaveChanges();
        return settings.DebugPausarBloqueio;
    }

    public bool DebugAtivo() => _db.Configuracoes.First().DebugPausarBloqueio;
}
