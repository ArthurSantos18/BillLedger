namespace BillLedger.UnitTests.Common.Customizations;

public class MoneyCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() =>
        {
            var raw = Math.Abs(fixture.Create<decimal>());
            var amount = Math.Round(raw % 999m + 1m, 2);
            return Money.Create(amount, Currency.Brl);
        });
    }
}
