using System.Text.RegularExpressions;

namespace CrecheManagement.Domain.Utils;

public static class Util
{
    public static bool IsCNPJ(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        var cnpj = Regex.Replace(input, @"[^\d]", "");

        if (cnpj.Length != 14)
            return false;

        if (cnpj.Distinct().Count() == 1)
            return false;

        int[] weightsFirstDigit = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] weightsSecondDigit = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var sum = 0;
        for (int i = 0; i < 12; i++)
            sum += (cnpj[i] - '0') * weightsFirstDigit[i];

        var remainder = sum % 11;
        var firstDigit = remainder < 2 ? 0 : 11 - remainder;

        if ((cnpj[12] - '0') != firstDigit)
            return false;

        sum = 0;
        for (int i = 0; i < 13; i++)
            sum += (cnpj[i] - '0') * weightsSecondDigit[i];

        remainder = sum % 11;
        var secondDigit = remainder < 2 ? 0 : 11 - remainder;

        return (cnpj[13] - '0') == secondDigit;
    }

    public static bool IsCEP(string input)
    {
        return Regex.IsMatch(input, @"^\d{5}-?\d{3}$");
    }

    public static bool IsPhoneNumber(string input)
    {
        input = KeepLettersAndNumbers(input);

        return Regex.IsMatch(input, @"^\d{2}9\d{8}$");
    }

    public static string KeepLettersAndNumbers(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return Regex.Replace(input, @"[^a-zA-Z0-9]", "");
    }
}