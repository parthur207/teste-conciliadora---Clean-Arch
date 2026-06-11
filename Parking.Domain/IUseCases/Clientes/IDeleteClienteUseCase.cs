using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Clientes;

public interface IDeleteClienteUseCase
{
    Task<SimpleResponseModel> ExecuteAsync(Guid id, CancellationToken ct = default);
}
