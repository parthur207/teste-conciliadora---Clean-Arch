using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Veiculos;

public record VeiculoListItem(Guid Id, string Placa, string? Modelo, int? Ano, Guid ClienteId, string? ClienteNome);

public interface IListVeiculosUseCase
{
    Task<ResponseModel<List<VeiculoListItem>>> ExecuteAsync(Guid? clienteId, CancellationToken ct = default);
}
