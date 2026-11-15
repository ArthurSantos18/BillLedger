namespace BillLedger.Domain.ValueObjects;

public sealed record Currency
{
    [Description("Representa o Real Brasileiro (BRL).")]
    public static readonly Currency Brl = new("BRL");

    [Description("Representa o Dólar Americano (USD).")]
    public static readonly Currency Usd = new("USD");

    [Description("Representa o Euro (EUR).")]
    public static readonly Currency Eur = new("EUR");

    [Description("O código ISO 4217 da moeda (BRL, USD ou EUR).")]
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
