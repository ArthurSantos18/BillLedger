namespace BillLedger.UnitTests.Common.Customizations;

public class DomainCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() => Invoice.Create(Guid.NewGuid(), $"INV-{fixture.Create<int>()}", fixture.Create<Money>(), DateTime.UtcNow.AddDays(30)));
        fixture.Register(() => Email.Create($"{Guid.NewGuid():N}@example.com"));
        fixture.Register(() => Cpf.Create(GenerateRandomValidCpf()));
        fixture.Register(() =>
        {
            var raw = Math.Abs(fixture.Create<decimal>());
            var amount = Math.Round(raw % 999m + 1m, 2);
            return Money.Create(amount, Currency.Brl);
        });

        fixture.Customize(new CustomerCustomization());
        fixture.Customize(new InvoiceCustomization());
    }

    private static string GenerateRandomValidCpf()
    {
        var random = new Random();
        var digits = new int[11];

        for (int i = 0; i < 9; i++)
            digits[i] = random.Next(0, 10);

        int sum1 = 0;
        for (int i = 0; i < 9; i++)
            sum1 += digits[i] * (10 - i);
        int remainder1 = sum1 % 11;
        digits[9] = remainder1 < 2 ? 0 : 11 - remainder1;

        int sum2 = 0;
        for (int i = 0; i < 10; i++)
            sum2 += digits[i] * (11 - i);
        int remainder2 = sum2 % 11;
        digits[10] = remainder2 < 2 ? 0 : 11 - remainder2;

        return string.Concat(digits);
    }
}
