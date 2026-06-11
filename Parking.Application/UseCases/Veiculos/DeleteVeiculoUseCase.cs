using Parking.Application.IRepositories;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Veiculos;
using Parking.Domain.ResponsePattern;

namespace Parking.Application.UseCases.Veiculos;

public class DeleteVeiculoUseCase : IDeleteVeiculoUseCase
{
    private readonly IVeiculoRepository _repository;

    public DeleteVeiculoUseCase(IVeiculoRepository repository) => _repository = repository;

    public async Task<SimpleResponseModel> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var veiculo = await _repository.GetByIdAsync(id, ct);
        if (veiculo is null)
            return new SimpleResponseModel("Veículo não encontrado.", ResponseStatusEnum.NotFound);

        _repository.Remove(veiculo);
        await _repository.SaveChangesAsync(ct);

        return new SimpleResponseModel("Veículo excluído com sucesso.", ResponseStatusEnum.Success);
    }
}
