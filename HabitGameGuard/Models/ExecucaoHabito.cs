using System.ComponentModel.DataAnnotations;

namespace HabitGameGuard.Models;

public class ExecucaoHabito
{
    [Key]
    public int Id { get; set; }
    public int HabitId { get; set; }
    public DateOnly Data { get; set; }
    public int VezesExecutadas { get; set; }
    public bool Completo { get; set; }
}
