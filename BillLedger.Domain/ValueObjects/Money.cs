namespace BillLedger.Domain.ValueObjects;

public sealed record Money
{
    [Description("O valor monetário, arredondado para duas casas decimais.")]
    public decimal Amount { get; }

    [Description("A moeda associada ao valor monetário.")]
    public Currency Currency { get; }

    private Money()
    {
        Amount = 0m;
        Currency = Currency.Brl;
    }

    private Money(decimal amount, Currency currency)
    {
        Amount = decimal.Round(amount, 2, MidpointRounding.ToEven);
        Currency = currency;
    }

    public static Money Create(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            throw new DomainException($"O valor não pode ser negativo. {amount}");
        }

        return new Money(amount, currency);
    }

    public static Money Zero(Currency currency)
    {
        return new Money(0m, currency);
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);

        if (other.Amount > Amount)
        {
            throw new DomainException($"Não é possível subtrair {other.Amount} de {Amount}: resultado seria negativo.");

        }

        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(int multiplier)
    {
        if (multiplier < 0)
        {
            throw new DomainException($"O multiplicador não pode ser negativo. {multiplier}");
        }

        return new Money(Amount * multiplier, Currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
        {
            throw new DomainException($"Não é possível operar moedas diferentes: {Currency.Code} vs {other.Currency.Code}");
        }
    }

    public override string ToString() => $"{Amount:F2} {Currency.Code}";
}
