using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Veiculos;
using Parking.Domain.ResponsePattern;

namespace Parking.Application.UseCases.Veiculos;

public class GetVeiculoByIdUseCase : IGetVeiculoByIdUseCase
{
    private readonly IVeiculoRepository _repository;

    public GetVeiculoByIdUseCase(IVeiculoRepository repository) => _repository = repository;

    public async Task<ResponseModel<VeiculoEntity>> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var veiculo = await _repository.GetByIdWithClienteAsync(id, ct);

        if(veiculo is null)
            return new ResponseModel<VeiculoEntity>(null, "Veículo não encontrado.", ResponseStatusEnum.NotFound);

        return new ResponseModel<VeiculoEntity>(veiculo, "Veículo encontrado.", ResponseStatusEnum.Success);
    }
}
