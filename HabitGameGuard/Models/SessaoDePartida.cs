using System.ComponentModel.DataAnnotations;

namespace HabitGameGuard.Models;

public class SessaoDePartida
{
    [Key]
    public int Id { get; set; }
    public int AplicativoBloqueadoId { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public int PartidasLiberadas { get; set; }
    public int PartidasConsumidas { get; set; }
    public string? UltimoGameIdDebitado { get; set; }
    public bool PartidaEmAndamento { get; set; }
    public string? GameIdAtual { get; set; }
    public DateTime? HoraInicioPartidaAtual { get; set; }
    public DateTime? HoraFimPartidaAtual { get; set; }
}
