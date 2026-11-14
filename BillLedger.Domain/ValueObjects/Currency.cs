namespace BillLedger.Domain.ValueObjects;

public sealed record Currency
{
    public static readonly Currency Brl = new("BRL");
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");

    public string Code { get; }

    private Currency(string code)
    {
        Code = code;
    }

    public static Currency FromCode(string code) => code.ToUpperInvariant() switch
    {
        "BRL" => Brl,
        "USD" => Usd,
        "EUR" => Eur,
        _ => throw new ArgumentException($"Moeda não suportada: {code}", nameof(code))
    };

    public override string ToString() => Code;
}
