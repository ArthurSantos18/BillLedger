namespace BillLedger.UnitTests.Common.Customizations;

public class DomainCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize(new MoneyCustomization());
        fixture.Register(() => Email.Create("user@example.com"));
        fixture.Register(() => Cpf.Create("12345678909"));
    }
}
