namespace Parking.Domain.ResponsePattern;

public record PagedResult<T>(int Total, IEnumerable<T> Itens);
