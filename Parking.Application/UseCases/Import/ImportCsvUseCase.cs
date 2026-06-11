using CsvHelper;
using CsvHelper.Configuration;
using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Import;
using Parking.Domain.ResponsePattern;
using Parking.Domain.ValueObjects;
using System.Globalization;
using System.Text;

namespace Parking.Application.UseCases.Import;

public class ImportCsvUseCase : IImportCsvUseCase
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly IVeiculoHistoricoRepository _historicoRepository;

    public ImportCsvUseCase(
        IClienteRepository clienteRepository,
        IVeiculoRepository veiculoRepository,
        IVeiculoHistoricoRepository historicoRepository)
    {
        _clienteRepository = clienteRepository;
        _veiculoRepository = veiculoRepository;
        _historicoRepository = historicoRepository;
    }

    public async Task<ResponseModel<ImportResult>> ExecuteAsync(Stream fileStream, CancellationToken ct = default)
    {
        using var reader = new StreamReader(fileStream, Encoding.UTF8);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            IgnoreBlankLines = true,
            HeaderValidated = null,
            MissingFieldFound = null
        };

        using var csv = new CsvReader(reader, config);

        int processados = 0;
        int inseridos = 0;
        var erros = new List<ImportErro>();
        int linhaArquivo = 1;

        try
        {
            await foreach (var registro in csv.GetRecordsAsync<VeiculoCsvDto>().WithCancellation(ct))
            {
                linhaArquivo++;
                processados++;

                var erro = await ProcessarRegistroAsync(registro, linhaArquivo, ct);

                if (erro is not null)
                    erros.Add(erro);
                else
                    inseridos++;
            }
        }
        catch (Exception ex)
        {
            erros.Add(new ImportErro(linhaArquivo, ex.Message));
        }

        return new ResponseModel<ImportResult>(
            new ImportResult(processados, inseridos, erros.Count, erros),
            null,
            ResponseStatusEnum.Success);
    }

    private async Task<ImportErro?> ProcessarRegistroAsync(VeiculoCsvDto registro, int linha, CancellationToken ct)
    {
        var placa = Placa.Sanitizar(registro.Placa);

        if (string.IsNullOrWhiteSpace(placa))
            return new ImportErro(linha, "Placa não informada.");

        if (!Placa.EhValida(placa))
            return new ImportErro(linha, $"Placa inválida: '{registro.Placa}'.");

        if (await _veiculoRepository.PlacaExistsAsync(placa, null, ct))
            return new ImportErro(linha, $"Placa '{placa}' já está cadastrada.");

        var cliNome = registro.ClienteNome?.Trim();
        if (string.IsNullOrWhiteSpace(cliNome))
            return new ImportErro(linha, "Nome do cliente não informado.");

        int? ano = int.TryParse(registro.Ano, out var anoVal) ? anoVal : null;

        var cliTel = new string(
            (registro.ClienteTelefone ?? string.Empty).Where(char.IsDigit).ToArray());

        bool mensalista =
            registro.Mensalista?.Equals("true", StringComparison.OrdinalIgnoreCase) == true
            || registro.Mensalista == "1";

        decimal? valorMens = null;
        if (!string.IsNullOrWhiteSpace(registro.ValorMensalidade))
        {
            if (!decimal.TryParse(registro.ValorMensalidade,
                    NumberStyles.Any, CultureInfo.InvariantCulture, out var vm))
                return new ImportErro(linha, $"Valor de mensalidade inválido: '{registro.ValorMensalidade}'.");

            valorMens = vm;
        }

        try
        {
            var cliente = await _clienteRepository.GetByNomeTelefoneAsync(cliNome, cliTel, ct);

            if (cliente is null)
            {
                cliente = new ClienteEntity
                {
                    Nome = cliNome,
                    Telefone = cliTel,
                    Endereco = registro.ClienteEndereco?.Trim(),
                    Mensalista = mensalista,
                    ValorMensalidade = valorMens
                };
                _clienteRepository.Add(cliente);
                await _clienteRepository.SaveChangesAsync(ct);
            }

            var veiculo = new VeiculoEntity
            {
                Placa = placa,
                Modelo = registro.Modelo,
                Ano = ano,
                ClienteId = cliente.Id
            };
            _veiculoRepository.Add(veiculo);

            _historicoRepository.Add(new VeiculoHistoricoEntity
            {
                VeiculoId = veiculo.Id,
                ClienteId = cliente.Id,
                DataInicio = DateTime.UtcNow.Date
            });

            await _veiculoRepository.SaveChangesAsync(ct);

            return null;
        }
        catch (Exception ex)
        {
            return new ImportErro(linha, $"Erro inesperado: {ex.Message}");
        }
    }
}
