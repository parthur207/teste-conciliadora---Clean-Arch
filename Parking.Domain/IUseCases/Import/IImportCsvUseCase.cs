using Parking.Domain.ResponsePattern;

namespace Parking.Domain.IUseCases.Import;

public record ImportErro(int Linha, string Motivo);
public record ImportResult(int Processados, int Inseridos, int TotalErros, List<ImportErro> Erros);

public interface IImportCsvUseCase
{
    Task<ResponseModel<ImportResult>> ExecuteAsync(Stream fileStream, CancellationToken ct = default);
}
