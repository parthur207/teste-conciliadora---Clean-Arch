using Microsoft.EntityFrameworkCore;
using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Infrastructure.Persistence;

namespace Parking.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly DbContextParking _db;

    public ClienteRepository(DbContextParking db) => _db = db;

    public async Task<(int Total, List<ClienteEntity> Itens)> ListAsync(int pagina, int tamanho, string? filtro, string mensalista, CancellationToken ct = default)
    {
        var q = _db.Clientes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
            q = q.Where(c => c.Nome.Contains(filtro));

        if (mensalista == "true") q = q.Where(c => c.Mensalista);
        if (mensalista == "false") q = q.Where(c => !c.Mensalista);

        var total = await q.CountAsync(ct);
        var itens = await q
            .OrderBy(c => c.Nome)
            .Skip((pagina - 1) * tamanho)
            .Take(tamanho)
            .ToListAsync(ct);

        return (total, itens);
    }

    public Task<ClienteEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Clientes.FindAsync(new object[] { id }, ct).AsTask();

    public Task<ClienteEntity?> GetByIdWithVeiculosAsync(Guid id, CancellationToken ct = default)
        => _db.Clientes.Include(x => x.Veiculos).FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<bool> ExistsByNomeTelefoneAsync(string nome, string? telefone, Guid? excludeId = null, CancellationToken ct = default)
        => _db.Clientes.AnyAsync(x =>
            (excludeId == null || x.Id != excludeId)
            && x.Nome.ToUpper().Trim() == nome
            && x.Telefone == telefone, ct);

    public Task<bool> HasVeiculosAsync(Guid id, CancellationToken ct = default)
        => _db.Veiculos.AnyAsync(v => v.ClienteId == id, ct);

    public Task<List<ClienteEntity>> GetMensalistasAsync(CancellationToken ct = default)
        => _db.Clientes.Where(c => c.Mensalista).AsNoTracking().ToListAsync(ct);

    public Task<ClienteEntity?> GetByNomeTelefoneAsync(string nome, string telefone, CancellationToken ct = default)
        => _db.Clientes.FirstOrDefaultAsync(c => c.Nome == nome && c.Telefone == telefone, ct);

    public void Add(ClienteEntity cliente) => _db.Clientes.Add(cliente);

    public void Remove(ClienteEntity cliente) => _db.Clientes.Remove(cliente);

    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
