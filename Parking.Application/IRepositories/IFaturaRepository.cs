using Parking.Domain.Entities;

namespace Parking.Application.IRepositories;

public interface IFaturaRepository
{
    Task<List<(FaturaEntity Fatura, string ClienteNome, int QtdVeiculos)>> ListWithClienteAsync(string? competencia, CancellationToken ct = default);
    Task<FaturaEntity?> GetByClienteCompetenciaAsync(Guid clienteId, string competencia, CancellationToken ct = default);
    void Add(FaturaEntity fatura);
    Task SaveChangesAsync(CancellationToken ct = default);
}
