using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Veiculos;
using Parking.Domain.ResponsePattern;
using Parking.Domain.ValueObjects;

namespace Parking.Application.UseCases.Veiculos;

public class CreateVeiculoUseCase : ICreateVeiculoUseCase
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly IVeiculoHistoricoRepository _historicoRepository;

    public CreateVeiculoUseCase(IVeiculoRepository veiculoRepository, IVeiculoHistoricoRepository historicoRepository)
    {
        _veiculoRepository = veiculoRepository;
        _historicoRepository = historicoRepository;
    }

    public async Task<ResponseModel<VeiculoEntity>> ExecuteAsync(VeiculoCreateInput input, CancellationToken ct = default)
    {
        var placa = Placa.Sanitizar(input.Placa);

        if (!Placa.EhValida(placa))
            return new ResponseModel<VeiculoEntity>(null, "Placa inválida. Formatos aceitos: ABC1234 (antigo) ou ABC1D23 (Mercosul).", ResponseStatusEnum.Error);
                
        if (await _veiculoRepository.PlacaExistsAsync(placa, null, ct))
            return new ResponseModel<VeiculoEntity>(null, "Placa já cadastrada.", ResponseStatusEnum.Conflict);

        if (!await _veiculoRepository.ClienteExistsAsync(input.ClienteId, ct))
            return new ResponseModel<VeiculoEntity>(null, "Cliente não encontrado.", ResponseStatusEnum.NotFound);

        var veiculo = new VeiculoEntity { Placa = placa, Modelo = input.Modelo, Ano = input.Ano, ClienteId = input.ClienteId };
        _veiculoRepository.Add(veiculo);

        _historicoRepository.Add(new VeiculoHistoricoEntity
        {
            VeiculoId = veiculo.Id,
            ClienteId = input.ClienteId,
            DataInicio = DateTime.UtcNow.Date
        });

        await _veiculoRepository.SaveChangesAsync(ct);

        return new ResponseModel<VeiculoEntity>(veiculo, "Veículo criado com sucesso.", ResponseStatusEnum.Success);
    }
}
