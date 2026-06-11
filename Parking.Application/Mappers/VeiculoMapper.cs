using Parking.Domain.Entities;
using Parking.Domain.IUseCases.Veiculos;

namespace Parking.Application.Mappers;

public static class VeiculoMapper
{
    public static VeiculoCreateInput ToCreateInput(Models.Veiculos.VeiculoCreateRequest request) =>
        new(request.Placa, request.Modelo, request.Ano, request.ClienteId);

    public static VeiculoUpdateInput ToUpdateInput(Guid id, Models.Veiculos.VeiculoUpdateRequest request) =>
        new(id, request.Placa, request.Modelo, request.Ano, request.ClienteId);

    public static VeiculoListItem ToListItem(VeiculoEntity entity) =>
        new(entity.Id, entity.Placa, entity.Modelo, entity.Ano, entity.ClienteId,
            entity.Cliente?.Nome);
}
