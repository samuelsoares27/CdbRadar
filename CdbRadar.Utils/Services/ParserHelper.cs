using System.Globalization;
using System.Text.RegularExpressions;

namespace CdbRadar.Utils.Services;

public static class ParserHelper
{
    // ✅ Converte "R$ 50.000,00" → 50000
    // Nunca retorna null: se falhar retorna 0
    public static decimal ParseMoney(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return 0;

        texto = texto.Replace("R$", "")
                     .Replace(".", "")
                     .Replace(",", ".")
                     .Trim();

        if (decimal.TryParse(texto, CultureInfo.InvariantCulture, out var valor))
            return valor;

        return 0;
    }

    // ✅ Converte "28/01/2036" → DateTime
    // Nunca retorna null: se falhar retorna hoje
    public static DateTime ParseDate(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return DateTime.Today;

        if (DateTime.TryParseExact(
                texto.Trim(),
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var data))
            return data;

        return DateTime.Today;
    }

    // ✅ Detecta indexador simples
    public static string DetectarIndexador(string taxa)
    {
        if (string.IsNullOrWhiteSpace(taxa))
            return "Outro";

        taxa = taxa.ToUpper();

        if (taxa.Contains("CDI")) return "CDI";
        if (taxa.Contains("IPCA")) return "IPCA";
        if (taxa.Contains("%")) return "Prefixado";

        return "Outro";
    }

    // ✅ Extrai valor numérico da taxa: "110% CDI" → 110
    // Se falhar retorna 0
    public static decimal ParseTaxaValor(string taxaTexto)
    {
        if (string.IsNullOrWhiteSpace(taxaTexto))
            return 0;

        // Remove letras e símbolos
        var limpo = new string(taxaTexto
            .Where(char.IsDigit)
            .ToArray());

        if (decimal.TryParse(limpo, out var valor))
            return valor;

        return 0;
    }
    public static decimal ExtrairNumero(string texto)
    {
        var match = Regex.Match(texto, @"\d+(,\d+)?");

        return match.Success
            ? decimal.Parse(match.Value, new CultureInfo("pt-BR"))
            : 0;
    }

}


