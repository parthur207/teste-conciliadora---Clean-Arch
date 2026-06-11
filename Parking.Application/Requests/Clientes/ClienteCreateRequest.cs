namespace Parking.Application.Models.Clientes;

public record ClienteCreateRequest(
    string Nome,
    string? Telefone,
    string? Endereco,
    bool Mensalista,
    decimal? ValorMensalidade);
