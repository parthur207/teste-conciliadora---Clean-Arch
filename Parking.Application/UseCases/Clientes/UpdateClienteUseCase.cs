using Parking.Application.IRepositories;
using Parking.Application.Mappers;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Clientes;
using Parking.Domain.ResponsePattern;
using System.Net.NetworkInformation;

namespace Parking.Application.UseCases.Clientes;

public class UpdateClienteUseCase : IUpdateClienteUseCase
{
    private readonly IClienteRepository _repository;

    public UpdateClienteUseCase(IClienteRepository repository) => _repository = repository;

    public async Task<ResponseModel<ClienteEntity>> ExecuteAsync(ClienteUpdateInput input, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(input.Nome))
            return new ResponseModel<ClienteEntity>(null, "O nome do cliente é obrigatório.", ResponseStatusEnum.Error)

        var cliente = await _repository.GetByIdAsync(input.Id, ct);

        if (cliente is null)
            return new ResponseModel<ClienteEntity>(null, "Cliente não encontrado.", ResponseStatusEnum.NotFound);

        var nomeFormatado = input.Nome.ToUpper().Trim();
        var telefoneFormatado = input.Telefone?.Trim();

        var conflito = await _repository.ExistsByNomeTelefoneAsync(nomeFormatado, telefoneFormatado, input.Id, ct);
        if (conflito)
            return new ResponseModel<ClienteEntity>(null, "Já existe outro cliente com esse nome e telefone.", ResponseStatusEnum.Conflict);

        ClienteMapper.ApplyUpdate(cliente, input);
        await _repository.SaveChangesAsync(ct);

        return new ResponseModel<ClienteEntity>(cliente, null, ResponseStatusEnum.Success);
    }
}
