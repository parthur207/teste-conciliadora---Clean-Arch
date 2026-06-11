using Parking.Domain.Entities;
using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Veiculos;

public interface IGetVeiculoByIdUseCase
{
    Task<ResponseModel<VeiculoEntity>> ExecuteAsync(Guid id, CancellationToken ct = default);
}
