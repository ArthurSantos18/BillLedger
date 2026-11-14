namespace BillLedger.Domain.ValueObjects;

public sealed record Cpf
{
    public string Value { get; }

    private Cpf(string value)
    {
        Value = value;
    }

    public static Cpf Create(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            throw new DomainException("CPF não pode ser vazio.");
        }

        var digits = new string([.. cpf.Where(char.IsAsciiDigit)]);

        if (digits.Length != 11)
        {
            throw new DomainException("CPF deve conter 11 dígitos.");
        }

        if (digits.Distinct().Count() == 1)
        {
            throw new DomainException("CPF não pode ter todos os dígitos iguais.");
        }

        if (!HasValidCheckDigits(digits))
        {
            throw new DomainException("Dígitos verificadores do CPF não conferem.");
        }

        return new Cpf(digits);
    }

    public override string ToString() => $"{Value[..3]}.{Value[3..6]}.{Value[6..9]}-{Value[9..]}";

    private static bool HasValidCheckDigits(string digits)
    {
        var firstSum = 0;
        for (var index = 0; index < 9; index++)
        {
            firstSum += (digits[index] - '0') * (10 - index);
        }

        var firstRemainder = firstSum % 11;
        var expectedFirst = firstRemainder < 2 ? 0 : 11 - firstRemainder;

        if (expectedFirst != digits[9] - '0')
        {
            return false;
        }

        var secondSum = 0;
        for (var index = 0; index < 10; index++)
        {
            secondSum += (digits[index] - '0') * (11 - index);
        }


        var secondRemainder = secondSum % 11;
        var expectedSecond = secondRemainder < 2 ? 0 : 11 - secondRemainder;

        return expectedSecond == digits[10] - '0';
    }
}
