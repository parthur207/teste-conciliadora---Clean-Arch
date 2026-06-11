using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Faturas;

public interface IGerarFaturaUseCase
{
    Task<ResponseModel<int>> ExecuteAsync(string competencia, CancellationToken ct = default);
}
