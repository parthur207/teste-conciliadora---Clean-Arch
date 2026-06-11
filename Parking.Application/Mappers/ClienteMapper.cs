using Parking.Domain.Entities;
using Parking.Domain.IUseCases.Clientes;

namespace Parking.Application.Mappers;

public static class ClienteMapper
{
    public static ClienteCreateInput ToCreateInput(Models.Clientes.ClienteCreateRequest request) =>
        new(request.Nome, request.Telefone, request.Endereco, request.Mensalista, request.ValorMensalidade);

    public static ClienteUpdateInput ToUpdateInput(Guid id, Models.Clientes.ClienteUpdateRequest request) =>
        new(id, request.Nome, request.Telefone, request.Endereco, request.Mensalista, request.ValorMensalidade);

    public static ClienteEntity ToEntity(ClienteCreateInput input) => new()
    {
        Nome = input.Nome,
        Telefone = input.Telefone?.Trim(),
        Endereco = input.Endereco,
        Mensalista = input.Mensalista,
        ValorMensalidade = input.ValorMensalidade
    };

    public static void ApplyUpdate(ClienteEntity entity, ClienteUpdateInput input)
    {
        entity.Nome = input.Nome;
        entity.Telefone = input.Telefone?.Trim();
        entity.Endereco = input.Endereco;
        entity.Mensalista = input.Mensalista;
        entity.ValorMensalidade = input.ValorMensalidade;
    }
}
