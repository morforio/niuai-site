namespace HabitGameGuard.Models;

public class AppSettings
{
    public int Id { get; set; }
    public string DatabasePath { get; set; } = string.Empty;
    public int IntervaloMonitoramentoSegundos { get; set; } = 5;
    public int SaldoCreditos { get; set; } = 0;
    public bool DebugPausarBloqueio { get; set; } = false;
}
