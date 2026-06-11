using Parking.Domain.Entities;
using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Clientes;

public record ClienteCreateInput(string Nome, string? Telefone, string? Endereco, bool Mensalista, decimal? ValorMensalidade);

public interface ICreateClienteUseCase
{
    Task<ResponseModel<ClienteEntity>> ExecuteAsync(ClienteCreateInput input, CancellationToken ct = default);
}
