using Parking.Domain.Entities;
using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Clientes;

public interface IGetClienteByIdUseCase
{
    Task<ResponseModel<ClienteEntity>> ExecuteAsync(Guid id, CancellationToken ct = default);
}
