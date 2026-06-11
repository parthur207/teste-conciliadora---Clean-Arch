using Parking.Application.IRepositories;
using Parking.Application.Mappers;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Faturas;
using Parking.Domain.ResponsePattern;

namespace Parking.Application.UseCases.Faturas;

public class ListFaturasUseCase : IListFaturasUseCase
{
    private readonly IFaturaRepository _repository;

    public ListFaturasUseCase(IFaturaRepository repository) => _repository = repository;

    public async Task<ResponseModel<List<FaturaListItem>>> ExecuteAsync(string? competencia, CancellationToken ct = default)
    {
        var resultados = await _repository.ListWithClienteAsync(competencia, ct);

        if (resultados is null || !resultados.Any())
            return new ResponseModel<List<FaturaListItem>>(null, "Nenhuma fatura encontrada para a competência informada.", ResponseStatusEnum.NotFound);

        var itens = resultados
            .Select(r => FaturaMapper.ToListItem(r.Fatura, r.ClienteNome, r.QtdVeiculos))
            .ToList();

        return new ResponseModel<List<FaturaListItem>>(itens, null, ResponseStatusEnum.Success);
    }
}
