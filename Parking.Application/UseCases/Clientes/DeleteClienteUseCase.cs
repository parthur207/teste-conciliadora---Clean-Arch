using Parking.Application.IRepositories;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Clientes;
using Parking.Domain.ResponsePattern;

namespace Parking.Application.UseCases.Clientes;

public class DeleteClienteUseCase : IDeleteClienteUseCase
{
    private readonly IClienteRepository _repository;

    public DeleteClienteUseCase(IClienteRepository repository) => _repository = repository;

    public async Task<SimpleResponseModel> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var cliente = await _repository.GetByIdAsync(id, ct);
        if (cliente is null)
            return new SimpleResponseModel("Cliente não encontrado.", ResponseStatusEnum.NotFound);

        var temVeiculos = await _repository.HasVeiculosAsync(id, ct);
        if (temVeiculos)
            return new SimpleResponseModel("Cliente possui veículos associados. Transfira ou remova os veículos antes de excluir.", ResponseStatusEnum.Error);

        _repository.Remove(cliente);
        await _repository.SaveChangesAsync(ct);

        return new SimpleResponseModel("Cliente excluído com sucesso.", ResponseStatusEnum.Success);
    }
}
