namespace Parking.Domain.Entities;

public class VeiculoHistoricoEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VeiculoId { get; set; }
    public Guid ClienteId { get; set; }
    public DateTime DataInicio { get; set; } = DateTime.UtcNow;
    public DateTime? DataFim { get; set; }
}
