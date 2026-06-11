namespace Parking.Application.Models.Clientes;

public record ClienteUpdateRequest(
    string Nome,
    string? Telefone,
    string? Endereco,
    bool Mensalista,
    decimal? ValorMensalidade);
