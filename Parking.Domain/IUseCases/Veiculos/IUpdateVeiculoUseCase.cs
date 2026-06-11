using Parking.Domain.Entities;
using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Veiculos;

public record VeiculoUpdateInput(Guid Id, string Placa, string? Modelo, int? Ano, Guid ClienteId);

public interface IUpdateVeiculoUseCase
{
    Task<ResponseModel<VeiculoEntity>> ExecuteAsync(VeiculoUpdateInput input, CancellationToken ct = default);
}
