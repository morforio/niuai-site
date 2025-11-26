using HabitGameGuard.Data;
using HabitGameGuard.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitGameGuard.Services;

public class HabitoService
{
    private readonly AppDbContext _db;
    private readonly CreditoService _creditoService;

    public HabitoService(AppDbContext db, CreditoService creditoService)
    {
        _db = db;
        _creditoService = creditoService;
        _db.Database.EnsureCreated();
    }

    public IEnumerable<Habit> ListarHabitosAtivos() => _db.Habitos.Where(h => h.Ativo).ToList();

    public IEnumerable<Habit> ListarTodos() => _db.Habitos.ToList();

    public void Salvar(Habit habit)
    {
        if (habit.Id == 0)
        {
            _db.Habitos.Add(habit);
        }
        else
        {
            _db.Habitos.Update(habit);
        }
        _db.SaveChanges();
    }

    public void Remover(Habit habit)
    {
        _db.Habitos.Remove(habit);
        _db.SaveChanges();
    }

    public int CalcularCreditosPorHabito(Dificuldade dificuldade) => dificuldade switch
    {
        Dificuldade.Facil => 1,
        Dificuldade.Medio => 2,
        Dificuldade.Dificil => 3,
        _ => 1
    };

    public ExecucaoHabito RegistrarExecucao(int habitId, DateOnly data)
    {
        var habit = _db.Habitos.FirstOrDefault(h => h.Id == habitId);
        if (habit is null)
        {
            throw new InvalidOperationException("Hábito não encontrado");
        }

        var execucao = _db.Execucoes.FirstOrDefault(e => e.HabitId == habitId && e.Data == data);
        if (execucao is null)
        {
            execucao = new ExecucaoHabito
            {
                HabitId = habitId,
                Data = data,
                VezesExecutadas = 1,
            };
            _db.Execucoes.Add(execucao);
        }
        else
        {
            execucao.VezesExecutadas++;
        }

        execucao.Completo = execucao.VezesExecutadas >= habit.MetaDiaria;
        _db.SaveChanges();

        if (execucao.Completo)
        {
            var creditos = CalcularCreditosPorHabito(habit.Dificuldade);
            _creditoService.AdicionarCreditos(creditos);
        }

        return execucao;
    }

    public int CalcularHabitosConcluidosHoje()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Now);
        return _db.Execucoes.Count(e => e.Data == hoje && e.Completo);
    }
}
