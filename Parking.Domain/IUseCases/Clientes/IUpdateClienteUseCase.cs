using Parking.Domain.Entities;
using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Clientes;

public record ClienteUpdateInput(Guid Id, string Nome, string? Telefone, string? Endereco, bool Mensalista, decimal? ValorMensalidade);

public interface IUpdateClienteUseCase
{
    Task<ResponseModel<ClienteEntity>> ExecuteAsync(ClienteUpdateInput input, CancellationToken ct = default);
}
