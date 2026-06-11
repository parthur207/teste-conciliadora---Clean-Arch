using Microsoft.AspNetCore.Mvc;
using Parking.Application.Mappers;
using Parking.Application.Models.Clientes;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Clientes;

namespace Parking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IListClientesUseCase _list;
    private readonly ICreateClienteUseCase _create;
    private readonly IGetClienteByIdUseCase _getById;
    private readonly IUpdateClienteUseCase _update;
    private readonly IDeleteClienteUseCase _delete;

    public ClientesController(
        IListClientesUseCase list,
        ICreateClienteUseCase create,
        IGetClienteByIdUseCase getById,
        IUpdateClienteUseCase update,
        IDeleteClienteUseCase delete)
    {
        _list = list;
        _create = create;
        _getById = getById;
        _update = update;
        _delete = delete;
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanho = 10,
        [FromQuery] string? filtro = null,
        [FromQuery] string mensalista = "all",
        CancellationToken ct = default)
    {
        var response = await _list.ExecuteAsync(new ListClientesInput(pagina, tamanho, filtro, mensalista), ct);

        return response.Status switch
        {
            ResponseStatusEnum.NotFound => NotFound(response),
            ResponseStatusEnum.Error => BadRequest(response),
            ResponseStatusEnum.Success => Ok(response),
            _ => BadRequest(response)
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClienteCreateRequest model, CancellationToken ct = default)
    {
        var response = await _create.ExecuteAsync(ClienteMapper.ToCreateInput(model), ct);

        return response.Status switch
        {
            ResponseStatusEnum.Error => BadRequest(response),
            ResponseStatusEnum.Conflict => Conflict(response),
            ResponseStatusEnum.Success => CreatedAtAction(nameof(GetById), new { id = response.Content!.Id }, response),
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
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ClienteUpdateRequest model, CancellationToken ct = default)
    {
        var response = await _update.ExecuteAsync(ClienteMapper.ToUpdateInput(id, model), ct);

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
            ResponseStatusEnum.Error => BadRequest(response),
            ResponseStatusEnum.Success => NoContent(),
            _ => BadRequest(response)
        };
    }
}
