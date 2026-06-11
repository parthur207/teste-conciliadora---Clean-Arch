using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Clientes;
using Parking.Domain.ResponsePattern;

namespace Parking.Application.UseCases.Clientes;

public class ListClientesUseCase : IListClientesUseCase
{
    private readonly IClienteRepository _repository;

    public ListClientesUseCase(IClienteRepository repository) => _repository = repository;

    public async Task<ResponseModel<PagedResult<ClienteEntity>>> ExecuteAsync(ListClientesInput input, CancellationToken ct = default)
    {
        var (total, itens) = await _repository.ListAsync(input.Pagina, input.Tamanho, input.Filtro, input.Mensalista, ct);
        return new ResponseModel<PagedResult<ClienteEntity>>(new PagedResult<ClienteEntity>(total, itens), null, ResponseStatusEnum.Success);
    }
}
