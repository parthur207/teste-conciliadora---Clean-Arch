using Parking.Domain.Entities;

namespace Parking.Application.IRepositories;

public interface IVeiculoHistoricoRepository
{
    Task<List<VeiculoHistoricoEntity>> GetByClienteForPeriodAsync(Guid clienteId, DateTime inicioMes, DateTime inicioProxMes, CancellationToken ct = default);
    Task<VeiculoHistoricoEntity?> GetActiveByVeiculoAsync(Guid veiculoId, CancellationToken ct = default);
    void Add(VeiculoHistoricoEntity historico);
    Task SaveChangesAsync(CancellationToken ct = default);
}
