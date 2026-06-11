namespace Parking.Domain.Entities;

public class VeiculoEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Placa { get; set; } = string.Empty;
    public string? Modelo { get; set; }
    public int? Ano { get; set; }
    public DateTime DataInclusao { get; set; } = DateTime.UtcNow;
    public Guid ClienteId { get; set; }
    public ClienteEntity? Cliente { get; set; }
}
