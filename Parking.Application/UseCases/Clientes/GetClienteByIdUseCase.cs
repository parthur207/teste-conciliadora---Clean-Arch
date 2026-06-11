using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Clientes;
using Parking.Domain.ResponsePattern;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;

namespace Parking.Application.UseCases.Clientes;

public class GetClienteByIdUseCase : IGetClienteByIdUseCase
{
    private readonly IClienteRepository _repository;

    public GetClienteByIdUseCase(IClienteRepository repository) => _repository = repository;

    public async Task<ResponseModel<ClienteEntity>> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var cliente = await _repository.GetByIdWithVeiculosAsync(id, ct);

        if (cliente is null)
            return new ResponseModel<ClienteEntity>(null, "Cliente não encontrado.", ResponseStatusEnum.NotFound);
               
        return new ResponseModel<ClienteEntity>(cliente, null, ResponseStatusEnum.Success);
    }
}
