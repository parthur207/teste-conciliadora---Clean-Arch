using System.Text.RegularExpressions;

namespace Parking.Domain.ValueObjects;

public static class Placa
{
    public static string Sanitizar(string? raw)
        => Regex.Replace(raw ?? string.Empty, "[^A-Za-z0-9]", string.Empty).ToUpperInvariant();

    public static bool EhValida(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return false;
        var s = Sanitizar(raw);
        return Regex.IsMatch(s, @"^[A-Z]{3}[0-9]{4}$")
            || Regex.IsMatch(s, @"^[A-Z]{3}[0-9][A-Z][0-9]{2}$");
    }
}
