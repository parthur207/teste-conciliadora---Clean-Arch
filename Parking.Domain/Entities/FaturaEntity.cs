namespace Parking.Domain.Entities;

public class FaturaEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Competencia { get; set; } = string.Empty;
    public Guid ClienteId { get; set; }
    public decimal Valor { get; set; }
    public DateTime CriadaEm { get; set; } = DateTime.UtcNow;
    public string? Observacao { get; set; }

    public List<FaturaVeiculoEntity> Veiculos { get; set; } = new();
}
