using HabitGameGuard.Data;
using HabitGameGuard.Models;

namespace HabitGameGuard.Services;

public class AplicativoBloqueadoService
{
    private readonly AppDbContext _db;

    public AplicativoBloqueadoService(AppDbContext db)
    {
        _db = db;
        _db.Database.EnsureCreated();
    }

    public IEnumerable<AplicativoBloqueado> Listar() => _db.Aplicativos.ToList();

    public AplicativoBloqueado? ObterPorId(int id) => _db.Aplicativos.FirstOrDefault(a => a.Id == id);

    public AplicativoBloqueado Salvar(AplicativoBloqueado app)
    {
        if (app.Id == 0)
        {
            _db.Aplicativos.Add(app);
        }
        else
        {
            _db.Aplicativos.Update(app);
        }
        _db.SaveChanges();
        return app;
    }

    public void Remover(AplicativoBloqueado app)
    {
        _db.Aplicativos.Remove(app);
        _db.SaveChanges();
    }
}
