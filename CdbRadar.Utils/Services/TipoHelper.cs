namespace CdbRadar.Utils.Services;

public static class TipoHelper
{
    public static string Detectar(string nome)
    {
        nome = nome.ToUpper();

        if (nome.StartsWith("CDB")) return "CDB";
        if (nome.StartsWith("LCI")) return "LCI";
        if (nome.StartsWith("LCA")) return "LCA";
        if (nome.StartsWith("LF")) return "LF";

        return "Outro";
    }
}
