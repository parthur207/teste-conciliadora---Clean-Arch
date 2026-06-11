using Parking.Application.IRepositories;
using Parking.Application.Mappers;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Veiculos;
using Parking.Domain.ResponsePattern;

namespace Parking.Application.UseCases.Veiculos;

public class ListVeiculosUseCase : IListVeiculosUseCase
{
    private readonly IVeiculoRepository _repository;

    public ListVeiculosUseCase(IVeiculoRepository repository) => _repository = repository;

    public async Task<ResponseModel<List<VeiculoListItem>>> ExecuteAsync(Guid? clienteId, CancellationToken ct = default)
    {
        var veiculos = await _repository.ListWithClienteAsync(clienteId, ct);

        var itens = veiculos.Select(VeiculoMapper.ToListItem).ToList();

        return new ResponseModel<List<VeiculoListItem>>(itens, "Veículos encontrados.", ResponseStatusEnum.Success);
    }
}
