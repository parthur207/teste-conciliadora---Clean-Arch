using Parking.Application.IRepositories;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Faturas;
using Parking.Domain.ResponsePattern;

namespace Parking.Application.UseCases.Faturas;

public class GetFaturaPlacasUseCase : IGetFaturaPlacasUseCase
{
    private readonly IFaturaVeiculoRepository _repository;

    public GetFaturaPlacasUseCase(IFaturaVeiculoRepository repository) => _repository = repository;

    public async Task<ResponseModel<List<string>>> ExecuteAsync(Guid faturaId, CancellationToken ct = default)
    {
        var placas = await _repository.GetPlacasByFaturaIdAsync(faturaId, ct);

        if(placas is null || !placas.Any())
            return new ResponseModel<List<string>>(null, "Nenhuma placa encontrada para a fatura especificada.", ResponseStatusEnum.NotFound);

        return new ResponseModel<List<string>>(placas, null, ResponseStatusEnum.Success);
    }
}
