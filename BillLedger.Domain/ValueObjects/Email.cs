namespace BillLedger.Domain.ValueObjects;

public sealed partial record Email
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
    private static partial Regex Format();

    [Description("O endereço de e-mail normalizado em letras minúsculas.")]
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("E-mail não pode ser vazio.");
        }

        var normalized = email.Trim().ToLowerInvariant();
        if (!Format().IsMatch(normalized))
        {
            throw new DomainException("Formato de email inválido.");
        }

        return new Email(normalized);
    }

    public override string ToString() => Value;
}