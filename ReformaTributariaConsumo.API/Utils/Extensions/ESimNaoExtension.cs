using ReformaTributaria.API.Utils.Enums;

namespace ReformaTributaria.API.Utils.Extensions;

public static class ESimNaoExtensions
{
    /// <summary>
    /// Retorna "S" para ESimNao.Sim e "N" para ESimNao.Não.
    /// Se o valor for null retorna "N".
    /// </summary>
    public static string ToSN(this ESimNao? value) =>
        (value ?? ESimNao.Não) switch
        {
            ESimNao.Sim => "S",
            _ => "N"
        };

    /// <summary>
    /// Versão para ESimNao não-nulo: retorna "S" para Sim e "N" para Não.
    /// </summary>
    public static string ToSN(this ESimNao value) =>
        value == ESimNao.Sim ? "S" : "N";
}
