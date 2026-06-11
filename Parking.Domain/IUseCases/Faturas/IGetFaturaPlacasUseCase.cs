using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Faturas;

public interface IGetFaturaPlacasUseCase
{
    Task<ResponseModel<List<string>>> ExecuteAsync(Guid faturaId, CancellationToken ct = default);
}
