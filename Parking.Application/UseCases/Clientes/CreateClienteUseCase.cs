using Parking.Application.IRepositories;
using Parking.Application.Mappers;
using Parking.Domain.Common;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Clientes;
using Parking.Domain.ResponsePattern;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;

namespace Parking.Application.UseCases.Clientes;

public class CreateClienteUseCase : ICreateClienteUseCase
{
    private readonly IClienteRepository _repository;

    public CreateClienteUseCase(IClienteRepository repository) => _repository = repository;

    public async Task<ResponseModel<ClienteEntity>> ExecuteAsync(ClienteCreateInput input, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(input.Nome))
            return new ResponseModel<ClienteEntity>(null, "Nome é obrigatório.", ResponseStatusEnum.Error);

        var nomeFormatado = input.Nome.ToUpper().Trim();
        var telefoneFormatado = input.Telefone?.Trim();

        var existe = await _repository.ExistsByNomeTelefoneAsync(nomeFormatado, telefoneFormatado, null, ct);
        if (existe)
            return new ResponseModel<ClienteEntity>(null, "Já existe um cliente com esse nome e telefone.", ResponseStatusEnum.Error);

        var cliente = ClienteMapper.ToEntity(input);
        _repository.Add(cliente);
        await _repository.SaveChangesAsync(ct);

        return new ResponseModel<ClienteEntity>(cliente, null, ResponseStatusEnum.Success);
    }
}
