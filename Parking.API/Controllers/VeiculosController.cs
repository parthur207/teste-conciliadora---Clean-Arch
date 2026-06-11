using Microsoft.AspNetCore.Mvc;
using Parking.Application.Mappers;
using Parking.Application.Models.Veiculos;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Veiculos;

namespace Parking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly IListVeiculosUseCase _list;
    private readonly ICreateVeiculoUseCase _create;
    private readonly IGetVeiculoByIdUseCase _getById;
    private readonly IUpdateVeiculoUseCase _update;
    private readonly IDeleteVeiculoUseCase _delete;

    public VeiculosController(
        IListVeiculosUseCase list,
        ICreateVeiculoUseCase create,
        IGetVeiculoByIdUseCase getById,
        IUpdateVeiculoUseCase update,
        IDeleteVeiculoUseCase delete)
    {
        _list = list;
        _create = create;
        _getById = getById;
        _update = update;
        _delete = delete;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] Guid? clienteId = null, CancellationToken ct = default)
    {
        var response = await _list.ExecuteAsync(clienteId, ct);

        return response.Status switch
        {
            ResponseStatusEnum.Success => Ok(response),
            _ => BadRequest(response)
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VeiculoCreateRequest model, CancellationToken ct = default)
    {
        var response = await _create.ExecuteAsync(VeiculoMapper.ToCreateInput(model), ct);

        return response.Status switch
        {
            ResponseStatusEnum.Error => BadRequest(response),
            ResponseStatusEnum.Conflict => Conflict(response),
            ResponseStatusEnum.Success => CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response),
            _ => BadRequest(response)
        };
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct = default)
    {
        var response = await _getById.ExecuteAsync(id, ct);

        return response.Status switch
        {
            ResponseStatusEnum.NotFound => NotFound(response),
            ResponseStatusEnum.Success => Ok(response),
            _ => BadRequest(response)
        };
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] VeiculoUpdateRequest model, CancellationToken ct = default)
    {
        var response = await _update.ExecuteAsync(VeiculoMapper.ToUpdateInput(id, model), ct);

        return response.Status switch
        {
            ResponseStatusEnum.Error => BadRequest(response),
            ResponseStatusEnum.NotFound => NotFound(response),
            ResponseStatusEnum.Conflict => Conflict(response),
            ResponseStatusEnum.Success => Ok(response),
            _ => BadRequest(response)
        };
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct = default)
    {
        var response = await _delete.ExecuteAsync(id, ct);

        return response.Status switch
        {
            ResponseStatusEnum.NotFound => NotFound(response),
            ResponseStatusEnum.Success => NoContent(),
            _ => BadRequest(response)
        };
    }
}
