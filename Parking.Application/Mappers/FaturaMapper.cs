using Parking.Domain.Entities;
using Parking.Domain.IUseCases.Faturas;

namespace Parking.Application.Mappers;

public static class FaturaMapper
{
    public static FaturaListItem ToListItem(FaturaEntity fatura, string clienteNome, int qtdVeiculos) =>
        new(fatura.Id, fatura.Competencia, fatura.ClienteId, clienteNome,
            fatura.Valor, fatura.CriadaEm, fatura.Observacao, qtdVeiculos);
}
