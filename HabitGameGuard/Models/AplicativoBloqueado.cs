using System.ComponentModel.DataAnnotations;

namespace HabitGameGuard.Models;

public class AplicativoBloqueado
{
    [Key]
    public int Id { get; set; }
    public string NomeAmigavel { get; set; } = string.Empty;
    public string NomeProcesso { get; set; } = string.Empty;
    public int CreditosPorPartida { get; set; }
    public bool Ativo { get; set; } = true;
}
