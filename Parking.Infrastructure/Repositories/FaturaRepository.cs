using Microsoft.EntityFrameworkCore;
using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Infrastructure.Persistence;

namespace Parking.Infrastructure.Repositories;

public class FaturaRepository : IFaturaRepository
{
    private readonly DbContextParking _db;

    public FaturaRepository(DbContextParking db) => _db = db;

    public async Task<List<(FaturaEntity Fatura, string ClienteNome, int QtdVeiculos)>> ListWithClienteAsync(string? competencia, CancellationToken ct = default)
    {
        var q = _db.Faturas.AsQueryable();
        if (!string.IsNullOrWhiteSpace(competencia))
            q = q.Where(f => f.Competencia == competencia);

        var resultados = await q
            .OrderByDescending(f => f.CriadaEm)
            .Join(_db.Clientes, f => f.ClienteId, c => c.Id, (f, c) => new
            {
                Fatura = f,
                ClienteNome = c.Nome,
                QtdVeiculos = f.Veiculos.Count
            })
            .ToListAsync(ct);

        return resultados.Select(r => (r.Fatura, r.ClienteNome, r.QtdVeiculos)).ToList();
    }

    public Task<FaturaEntity?> GetByClienteCompetenciaAsync(Guid clienteId, string competencia, CancellationToken ct = default)
        => _db.Faturas.FirstOrDefaultAsync(f => f.ClienteId == clienteId && f.Competencia == competencia, ct);

    public void Add(FaturaEntity fatura) => _db.Faturas.Add(fatura);

    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
