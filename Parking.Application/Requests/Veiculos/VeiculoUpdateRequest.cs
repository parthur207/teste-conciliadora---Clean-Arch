namespace Parking.Application.Models.Veiculos;

public record VeiculoUpdateRequest(string Placa, string? Modelo, int? Ano, Guid ClienteId);
