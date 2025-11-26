using HabitGameGuard.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitGameGuard.Data;

public class AppDbContext : DbContext
{
    public DbSet<Habit> Habitos => Set<Habit>();
    public DbSet<ExecucaoHabito> Execucoes => Set<ExecucaoHabito>();
    public DbSet<AplicativoBloqueado> Aplicativos => Set<AplicativoBloqueado>();
    public DbSet<SessaoDePartida> Sessoes => Set<SessaoDePartida>();
    public DbSet<AppSettings> Configuracoes => Set<AppSettings>();

    private readonly string _dbPath;

    public AppDbContext()
    {
        var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HabitGameGuard");
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }
        _dbPath = Path.Combine(basePath, "habitguard.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppSettings>().HasData(new AppSettings
        {
            Id = 1,
            DatabasePath = _dbPath,
            IntervaloMonitoramentoSegundos = 5,
            SaldoCreditos = 0,
            DebugPausarBloqueio = false
        });
    }
}
