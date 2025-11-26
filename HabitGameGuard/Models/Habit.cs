using System.ComponentModel.DataAnnotations;

namespace HabitGameGuard.Models;

public class Habit
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public Dificuldade Dificuldade { get; set; }
    public int MetaDiaria { get; set; }
    public bool Ativo { get; set; } = true;
}
