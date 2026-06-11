using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Veiculos;
using Parking.Domain.ResponsePattern;
using Parking.Domain.ValueObjects;

namespace Parking.Application.UseCases.Veiculos;

public class UpdateVeiculoUseCase : IUpdateVeiculoUseCase
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly IVeiculoHistoricoRepository _historicoRepository;

    public UpdateVeiculoUseCase(IVeiculoRepository veiculoRepository, IVeiculoHistoricoRepository historicoRepository)
    {
        _veiculoRepository = veiculoRepository;
        _historicoRepository = historicoRepository;
    }

    public async Task<ResponseModel<VeiculoEntity>> ExecuteAsync(VeiculoUpdateInput input, CancellationToken ct = default)
    {
        var veiculo = await _veiculoRepository.GetByIdAsync(input.Id, ct);

        if (veiculo is null)
            return new ResponseModel<VeiculoEntity>(null, "Veículo não encontrado.", ResponseStatusEnum.NotFound);

        var placa = Placa.Sanitizar(input.Placa);

        if (!Placa.EhValida(placa))
            return new ResponseModel<VeiculoEntity>(null, "Placa inválida.", ResponseStatusEnum.Error);

        if (await _veiculoRepository.PlacaExistsAsync(placa, input.Id, ct))
            return new ResponseModel<VeiculoEntity>(null, "Placa já cadastrada para outro veículo.", ResponseStatusEnum.Conflict);

        if (!await _veiculoRepository.ClienteExistsAsync(input.ClienteId, ct))
            return new ResponseModel<VeiculoEntity>(null, "Cliente não encontrado.", ResponseStatusEnum.Error);

        if (veiculo.ClienteId != input.ClienteId)
        {
            var hoje = DateTime.UtcNow.Date;

            var histAtual = await _historicoRepository.GetActiveByVeiculoAsync(input.Id, ct);
            if (histAtual is not null)
                histAtual.DataFim = hoje;

            _historicoRepository.Add(new VeiculoHistoricoEntity
            {
                VeiculoId = veiculo.Id,
                ClienteId = input.ClienteId,
                DataInicio = hoje
            });
        }

        veiculo.Placa = placa;
        veiculo.Modelo = input.Modelo;
        veiculo.Ano = input.Ano;
        veiculo.ClienteId = input.ClienteId;

        await _veiculoRepository.SaveChangesAsync(ct);

        return new ResponseModel<VeiculoEntity>(veiculo, "Veículo atualizado.", ResponseStatusEnum.Success);
    }
}
