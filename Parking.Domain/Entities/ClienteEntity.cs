namespace Parking.Domain.Entities;

public class ClienteEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Endereco { get; set; }
    public bool Mensalista { get; set; }
    public decimal? ValorMensalidade { get; set; }
    public DateTime DataInclusao { get; set; } = DateTime.UtcNow;

    public List<VeiculoEntity> Veiculos { get; set; } = new();
}
