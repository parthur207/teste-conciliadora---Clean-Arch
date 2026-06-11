using Parking.Domain.Entities;
using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Clientes;

public record ListClientesInput(int Pagina = 1, int Tamanho = 10, string? Filtro = null, string Mensalista = "all");

public interface IListClientesUseCase
{
    Task<ResponseModel<PagedResult<ClienteEntity>>> ExecuteAsync(ListClientesInput input, CancellationToken ct = default);
}
