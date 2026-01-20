using System.Text.RegularExpressions;

namespace ReformaTributaria.API.Utils;

public static partial class StringExtensions
{
    public static string SafeSubstring(this string value, int startIndex, int length)
    {
        return new string([.. (value ?? string.Empty).Skip(startIndex).Take(length)]);
    }

    public static string SoNumeros(this string value) => RegExSonumero().Replace(value, "");

    public static string FormataCPF(this string value)
    {
        return value.SafeSubstring(0, 3) + '.' + value.SafeSubstring(3, 3) + '.' + value.SafeSubstring(6, 3) +
            '-' + value.SafeSubstring(9, 2);
    }

    public static string FormataCNPJ(this string value)
    {
        return value.SafeSubstring(0, 2) + '.' + value.SafeSubstring(2, 3) + '.' + value.SafeSubstring(5, 3) +
            '/' + value.SafeSubstring(8, 4) + '-' + value.SafeSubstring(12, 2);
    }

    public static string AddOnde(this string value, string onde)
    {
        if (onde == "") return value;

        if (value == "")
        {
            value = " where ";
        }
        else
        {
            value += " and ";
        }

        return value + onde;
    }

    public static object? ToDateOnly(this string value)
    {
        if (value == string.Empty) return DBNull.Value;

        var dataRetorno = DateOnly.ParseExact(value, "yyyy-MM-dd");
        return dataRetorno;
    }

    public static bool ValidaCNPJ(this string value)
    {
        int[] multiplicador1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int soma = 0;
        int resto = 0;
        string digito = "";
        string tempCnpj = "";

        string cnpj = value.SoNumeros();

        if (cnpj.Length != 14)
            return false;

        // deixando passar o cnpj de testes
        if (Cnpj.CnpjLiberado(cnpj))
            return true;

        tempCnpj = cnpj.SafeSubstring(0, 12);

        for (var i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = resto.ToString();
        tempCnpj += digito;
        soma = 0;

        for (var i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito += resto.ToString();

        return cnpj.EndsWith(digito);
    }

    public static bool ValidaCPF(this string value)
    {
        int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
        string tempCpf;
        string digito;
        int soma;
        int resto;

        string cpf = value.SoNumeros();

        if (cpf.Length != 11)
        {
            return false;
        }

        tempCpf = cpf.SafeSubstring(0, 9);
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = resto.ToString();
        tempCpf += digito;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito += resto.ToString();

        return cpf.EndsWith(digito);
    }

    public static bool ValidaPIS(this string value)
    {
        int[] multiplicador = [3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int soma = 0;
        int resto = 0;

        string pis = value.SoNumeros().Trim();

        if (pis.Length != 11)
            return false;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(pis[i].ToString()) * multiplicador[i];

        resto = soma % 11;

        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        return pis.EndsWith(resto.ToString());
    }

    [GeneratedRegex("[^\\d]")]
    private static partial Regex RegExSonumero();
}