using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Faturas;
using Parking.Domain.ResponsePattern;

namespace Parking.Application.UseCases.Faturas;

public class GerarFaturaUseCase : IGerarFaturaUseCase
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IFaturaRepository _faturaRepository;
    private readonly IVeiculoHistoricoRepository _historicoRepository;

    public GerarFaturaUseCase(
        IClienteRepository clienteRepository,
        IFaturaRepository faturaRepository,
        IVeiculoHistoricoRepository historicoRepository)
    {
        _clienteRepository = clienteRepository;
        _faturaRepository = faturaRepository;
        _historicoRepository = historicoRepository;
    }

    public async Task<ResponseModel<int>> ExecuteAsync(string competencia, CancellationToken ct = default)
    {
        var Response= new ResponseModel<int>();

        if (string.IsNullOrWhiteSpace(competencia))
            return new ResponseModel<int>(0, "O formato informado não pode ser nulo. Opte por enviar conforme exemplo a seguir: (ano-mes) => 2026-06", ResponseStatusEnum.Error);

        var partes = competencia.Split('-');
        if (partes.Length != 2 || !int.TryParse(partes[0], out var ano) || !int.TryParse(partes[1], out var mes))
            return new ResponseModel<int>(0, "Competência deve estar no formato yyyy-MM.", ResponseStatusEnum.Error);

        var diasNoMes = DateTime.DaysInMonth(ano, mes);
        var inicioMes = new DateTime(ano, mes, 1, 0, 0, 0, DateTimeKind.Utc);
        var inicioProxMes = inicioMes.AddMonths(1);

        var mensalistas = await _clienteRepository.GetMensalistasAsync(ct);
        var criadas = 0;

        foreach (var cli in mensalistas)
        {
            var existente = await _faturaRepository.GetByClienteCompetenciaAsync(cli.Id, competencia, ct);
            if (existente is not null) continue;

            var historicos = await _historicoRepository.GetByClienteForPeriodAsync(cli.Id, inicioMes, inicioProxMes, ct);
            if (!historicos.Any()) continue;

            var taxaDiaria = (cli.ValorMensalidade ?? 0m) / diasNoMes;
            decimal valorTotal = 0m;
            var veiculoIds = new List<Guid>();

            foreach (var h in historicos)
            {
                var inicioDia = h.DataInicio.Date > inicioMes.Date ? h.DataInicio.Date : inicioMes.Date;
                var fimDiaExclusivo = h.DataFim.HasValue
                    ? (h.DataFim.Value.Date < inicioProxMes.Date ? h.DataFim.Value.Date : inicioProxMes.Date)
                    : inicioProxMes.Date;

                var dias = (int)(fimDiaExclusivo - inicioDia).TotalDays;
                if (dias <= 0) continue;

                valorTotal += taxaDiaria * dias;

                if (!veiculoIds.Contains(h.VeiculoId))
                    veiculoIds.Add(h.VeiculoId);
            }

            if (valorTotal <= 0) continue;

            var fatura = new FaturaEntity
            {
                Competencia = competencia,
                ClienteId = cli.Id,
                Valor = Math.Round(valorTotal, 2),
                Observacao = $"Proporcional: {veiculoIds.Count} veículo(s) em {competencia}"
            };

            foreach (var vid in veiculoIds)
                fatura.Veiculos.Add(new FaturaVeiculoEntity { FaturaId = fatura.Id, VeiculoId = vid });

            _faturaRepository.Add(fatura);
            criadas++;
        }

        await _faturaRepository.SaveChangesAsync(ct);

        return new ResponseModel<int>(criadas, $"Faturas geradas para {criadas} cliente(s) na competência {competencia}.", ResponseStatusEnum.Success);
    }
}
