using Microsoft.EntityFrameworkCore;
using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Infrastructure.Persistence;

namespace Parking.Infrastructure.Repositories;

public class VeiculoHistoricoRepository : IVeiculoHistoricoRepository
{
    private readonly DbContextParking _db;

    public VeiculoHistoricoRepository(DbContextParking db) => _db = db;

    public Task<List<VeiculoHistoricoEntity>> GetByClienteForPeriodAsync(Guid clienteId, DateTime inicioMes, DateTime inicioProxMes, CancellationToken ct = default)
        => _db.VeiculosHistorico
            .Where(h => h.ClienteId == clienteId
                && h.DataInicio < inicioProxMes
                && (h.DataFim == null || h.DataFim > inicioMes))
            .AsNoTracking()
            .ToListAsync(ct);

    public Task<VeiculoHistoricoEntity?> GetActiveByVeiculoAsync(Guid veiculoId, CancellationToken ct = default)
        => _db.VeiculosHistorico.FirstOrDefaultAsync(h => h.VeiculoId == veiculoId && h.DataFim == null, ct);

    public void Add(VeiculoHistoricoEntity historico) => _db.VeiculosHistorico.Add(historico);

    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
