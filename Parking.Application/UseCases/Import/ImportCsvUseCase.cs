using Parking.Application.IRepositories;
using Parking.Domain.Entities;
using Parking.Domain.Enums;
using Parking.Domain.IUseCases.Import;
using Parking.Domain.ResponsePattern;
using Parking.Domain.ValueObjects;
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

        int linhaArquivo = 1;
        int processados = 0;
        int inseridos = 0;
        var erros = new List<ImportErro>();

        await reader.ReadLineAsync();

        while (!reader.EndOfStream)
        {
            linhaArquivo++;
            var raw = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(raw)) continue;
            processados++;

            var cols = raw.Split(',');

            if (cols.Length < 9)
            {
                erros.Add(new ImportErro(linhaArquivo, $"Número de colunas inválido (esperado 9, encontrado {cols.Length})."));
                continue;
            }

            var placaRaw = cols[0].Trim();
            var modelo = cols[1].Trim();
            var anoStr = cols[2].Trim();
            var cliNome = cols[4].Trim();
            var cliTelRaw = cols[5].Trim();
            var cliEnd = cols[6].Trim();
            var mensalistaStr = cols[7].Trim();
            var valorMensStr = cols[8].Trim();

            var placa = Placa.Sanitizar(placaRaw);

            if (string.IsNullOrWhiteSpace(placa))
            {
                erros.Add(new ImportErro(linhaArquivo, "Placa não informada."));
                continue;
            }

            if (!Placa.EhValida(placa))
            {
                erros.Add(new ImportErro(linhaArquivo, $"Placa inválida: '{placaRaw}'."));
                continue;
            }

            if (await _veiculoRepository.PlacaExistsAsync(placa, null, ct))
            {
                erros.Add(new ImportErro(linhaArquivo, $"Placa '{placa}' já está cadastrada."));
                continue;
            }

            if (string.IsNullOrWhiteSpace(cliNome))
            {
                erros.Add(new ImportErro(linhaArquivo, "Nome do cliente não informado."));
                continue;
            }

            int? ano = int.TryParse(anoStr, out var anoVal) ? anoVal : null;
            var cliTel = new string(cliTelRaw.Where(char.IsDigit).ToArray());
            bool mensalista = bool.TryParse(mensalistaStr, out var mBool) && mBool;

            decimal? valorMens = null;
            if (!string.IsNullOrWhiteSpace(valorMensStr))
            {
                if (!decimal.TryParse(valorMensStr, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out var vm))
                {
                    erros.Add(new ImportErro(linhaArquivo, $"Valor de mensalidade inválido: '{valorMensStr}'."));
                    continue;
                }
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
                        Endereco = cliEnd,
                        Mensalista = mensalista,
                        ValorMensalidade = valorMens
                    };
                    _clienteRepository.Add(cliente);
                    await _clienteRepository.SaveChangesAsync(ct);
                }

                var veiculo = new VeiculoEntity { Placa = placa, Modelo = modelo, Ano = ano, ClienteId = cliente.Id };
                _veiculoRepository.Add(veiculo);

                _historicoRepository.Add(new VeiculoHistoricoEntity
                {
                    VeiculoId = veiculo.Id,
                    ClienteId = cliente.Id,
                    DataInicio = DateTime.UtcNow.Date
                });

                await _veiculoRepository.SaveChangesAsync(ct);
                inseridos++;
            }
            catch (Exception ex)
            {
                erros.Add(new ImportErro(linhaArquivo, $"Erro inesperado: {ex.Message}"));
            }
        }
        return new ResponseModel<ImportResult>(new ImportResult
        {
             = processados,
            TotalInseridos = inseridos,
            Erros = erros
        }, null, status: ResponseStatusEnum.Success);

    }
}
