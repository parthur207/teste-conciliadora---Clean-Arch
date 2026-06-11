using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Veiculos;

public interface IDeleteVeiculoUseCase
{
    Task<SimpleResponseModel> ExecuteAsync(Guid id, CancellationToken ct = default);
}
