using Microsoft.EntityFrameworkCore;
using Parking.Application.IRepositories;
using Parking.Infrastructure.Persistence;

namespace Parking.Infrastructure.Repositories;

public class FaturaVeiculoRepository : IFaturaVeiculoRepository
{
    private readonly DbContextParking _db;

    public FaturaVeiculoRepository(DbContextParking db) => _db = db;

    public Task<List<string>> GetPlacasByFaturaIdAsync(Guid faturaId, CancellationToken ct = default)
        => _db.FaturasVeiculos
            .Where(x => x.FaturaId == faturaId)
            .Join(_db.Veiculos, fv => fv.VeiculoId, v => v.Id, (fv, v) => v.Placa)
            .ToListAsync(ct);
}
