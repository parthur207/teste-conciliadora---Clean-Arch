using Microsoft.AspNetCore.Mvc;
using Parking.Application.Models.Faturas;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Faturas;

namespace Parking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FaturasController : ControllerBase
{
    private readonly IGerarFaturaUseCase _gerar;
    private readonly IListFaturasUseCase _list;
    private readonly IGetFaturaPlacasUseCase _placas;

    public FaturasController(
        IGerarFaturaUseCase gerar,
        IListFaturasUseCase list,
        IGetFaturaPlacasUseCase placas)
    {
        _gerar = gerar;
        _list = list;
        _placas = placas;
    }

    [HttpPost("gerar")]
    public async Task<IActionResult> Gerar([FromBody] GerarFaturaRequest req, CancellationToken ct = default)
    {
        var response = await _gerar.ExecuteAsync(req.Competencia, ct);

        return response.Status switch
        {
            ResponseStatusEnum.Error => BadRequest(response),
            ResponseStatusEnum.Success => Ok(response),
            _ => BadRequest(response)
        };
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? competencia = null, CancellationToken ct = default)
    {
        var response = await _list.ExecuteAsync(competencia, ct);

        return response.Status switch
        {
            ResponseStatusEnum.Success => Ok(response),
            _ => BadRequest(response)
        };
    }

    [HttpGet("{id:guid}/placas")]
    public async Task<IActionResult> Placas([FromRoute] Guid id, CancellationToken ct = default)
    {
        var response = await _placas.ExecuteAsync(id, ct);

        return response.Status switch
        {
            ResponseStatusEnum.Success => Ok(response),
            _ => BadRequest(response)
        };
    }
}
