namespace Parking.Application.Models.Veiculos;

public record VeiculoCreateRequest(string Placa, string? Modelo, int? Ano, Guid ClienteId);
