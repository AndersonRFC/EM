using System.Text.RegularExpressions;

namespace EM.Domain.Utilitary;

public static class Validador
{
    public static bool ValidarCPF(this string cpf)
    {
        cpf = Regex.Replace(cpf, @"\D", "");

        if (cpf.Length != 11)
            return false;

        if (new string(cpf[0], 11) == cpf)
            return false;

        int soma = 0;
        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(cpf[i].ToString()) * (10 - i);
        }
        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (digito1 != int.Parse(cpf[9].ToString()))
            return false;

        soma = 0;
        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(cpf[i].ToString()) * (11 - i);
        }
        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        if (digito2 != int.Parse(cpf[10].ToString()))
            return false;

        return true;
    }
}
