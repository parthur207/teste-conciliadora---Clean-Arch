namespace Parking.Application.IRepositories;

public interface IFaturaVeiculoRepository
{
    Task<List<string>> GetPlacasByFaturaIdAsync(Guid faturaId, CancellationToken ct = default);
}
