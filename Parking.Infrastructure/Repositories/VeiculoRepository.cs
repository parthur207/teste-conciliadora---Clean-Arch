using Microsoft.EntityFrameworkCore;
using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Infrastructure.Persistence;

namespace Parking.Infrastructure.Repositories;

public class VeiculoRepository : IVeiculoRepository
{
    private readonly DbContextParking _db;

    public VeiculoRepository(DbContextParking db) => _db = db;

    public Task<List<VeiculoEntity>> ListWithClienteAsync(Guid? clienteId, CancellationToken ct = default)
    {
        var q = _db.Veiculos.Include(v => v.Cliente).AsQueryable();
        if (clienteId.HasValue) q = q.Where(v => v.ClienteId == clienteId.Value);
        return q.OrderBy(v => v.Placa).ToListAsync(ct);
    }

    public Task<VeiculoEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Veiculos.FindAsync(new object[] { id }, ct).AsTask();

    public Task<VeiculoEntity?> GetByIdWithClienteAsync(Guid id, CancellationToken ct = default)
        => _db.Veiculos.Include(x => x.Cliente).FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<bool> PlacaExistsAsync(string placa, Guid? excludeId = null, CancellationToken ct = default)
        => _db.Veiculos.AnyAsync(v => v.Placa == placa && (excludeId == null || v.Id != excludeId), ct);

    public Task<bool> ClienteExistsAsync(Guid clienteId, CancellationToken ct = default)
        => _db.Clientes.AnyAsync(c => c.Id == clienteId, ct);

    public void Add(VeiculoEntity veiculo) => _db.Veiculos.Add(veiculo);

    public void Remove(VeiculoEntity veiculo) => _db.Veiculos.Remove(veiculo);

    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
