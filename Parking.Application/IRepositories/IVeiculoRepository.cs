using Parking.Domain.Entities;

namespace Parking.Application.IRepositories;

public interface IVeiculoRepository
{
    Task<List<VeiculoEntity>> ListWithClienteAsync(Guid? clienteId, CancellationToken ct = default);
    Task<VeiculoEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<VeiculoEntity?> GetByIdWithClienteAsync(Guid id, CancellationToken ct = default);
    Task<bool> PlacaExistsAsync(string placa, Guid? excludeId = null, CancellationToken ct = default);
    Task<bool> ClienteExistsAsync(Guid clienteId, CancellationToken ct = default);
    void Add(VeiculoEntity veiculo);
    void Remove(VeiculoEntity veiculo);
    Task SaveChangesAsync(CancellationToken ct = default);
}
