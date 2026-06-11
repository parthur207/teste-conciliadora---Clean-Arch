using Microsoft.AspNetCore.Mvc;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Import;

namespace Parking.Api.Controllers;

[ApiController]
[Route("api/import")]
public class ImportController : ControllerBase
{
    private readonly IImportCsvUseCase _importCsv;

    public ImportController(IImportCsvUseCase importCsv) => _importCsv = importCsv;

    [HttpPost("csv")]
    public async Task<IActionResult> ImportCsv([FromForm] IFormFile file, CancellationToken ct = default)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Envie um arquivo CSV no campo 'file'.");

        if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Arquivo inválido. Apenas arquivos .csv são permitidos.");

        using var stream = file.OpenReadStream();
        var response = await _importCsv.ExecuteAsync(stream, ct);

        return response.Status switch
        {
            ResponseStatusEnum.Error => BadRequest(response),
            ResponseStatusEnum.CriticalError => BadRequest(response),
            ResponseStatusEnum.Success => Ok(response),
            _ => BadRequest(response)
        };
    }
}
