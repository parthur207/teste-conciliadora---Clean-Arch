using CsvHelper.Configuration.Attributes;

namespace Parking.Application.UseCases.Import;

public class VeiculoCsvDto
{
    [Name("Placa")]
    public string? Placa { get; set; }

    [Name("Modelo")]
    public string? Modelo { get; set; }

    [Name("Ano")]
    public string? Ano { get; set; }

    [Name("ClienteNome")]
    public string? ClienteNome { get; set; }

    [Name("ClienteTelefone")]
    public string? ClienteTelefone { get; set; }

    [Name("ClienteEndereco")]
    public string? ClienteEndereco { get; set; }

    [Name("Mensalista")]
    public string? Mensalista { get; set; }

    [Name("ValorMensalidade")]
    public string? ValorMensalidade { get; set; }
}
