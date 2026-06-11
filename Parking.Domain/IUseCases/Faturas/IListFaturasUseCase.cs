using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Faturas;

public record FaturaListItem(
    Guid Id,
    string Competencia,
    Guid ClienteId,
    string ClienteNome,
    decimal Valor,
    DateTime CriadaEm,
    string? Observacao,
    int QtdVeiculos);

public interface IListFaturasUseCase
{
    Task<ResponseModel<List<FaturaListItem>>> ExecuteAsync(string? competencia, CancellationToken ct = default);
}
