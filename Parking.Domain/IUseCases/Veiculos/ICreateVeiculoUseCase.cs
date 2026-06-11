using Parking.Domain.Entities;
using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Veiculos;

public record VeiculoCreateInput(string Placa, string? Modelo, int? Ano, Guid ClienteId);

public interface ICreateVeiculoUseCase
{
    Task<ResponseModel<VeiculoEntity>> ExecuteAsync(VeiculoCreateInput input, CancellationToken ct = default);
}
