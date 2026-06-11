using Parking.Domain.Entities;

namespace Parking.Application.IRepositories;

public interface IClienteRepository
{
    Task<(int Total, List<ClienteEntity> Itens)> ListAsync(int pagina, int tamanho, string? filtro, string mensalista, CancellationToken ct = default);
    Task<ClienteEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ClienteEntity?> GetByIdWithVeiculosAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByNomeTelefoneAsync(string nome, string? telefone, Guid? excludeId = null, CancellationToken ct = default);
    Task<bool> HasVeiculosAsync(Guid id, CancellationToken ct = default);
    Task<List<ClienteEntity>> GetMensalistasAsync(CancellationToken ct = default);
    Task<ClienteEntity?> GetByNomeTelefoneAsync(string nome, string telefone, CancellationToken ct = default);
    void Add(ClienteEntity cliente);
    void Remove(ClienteEntity    cliente);
    Task SaveChangesAsync(CancellationToken ct = default);
}
