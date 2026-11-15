namespace BillLedger.UnitTests.Common.Customizations;

public class InvoiceCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<CreateInvoiceRequest>(c => c
            .With(r => r.CustomerId, () => Guid.NewGuid())
            .With(r => r.Number, () => $"INV-{fixture.Create<int>()}")
            .With(r => r.Amount, () => Math.Round((decimal)fixture.Create<double>() % 1000m + 1m, 2))
            .With(r => r.CurrencyCode, "BRL")
            .With(r => r.DueDate, () => DateTime.UtcNow.AddDays(15)));

        fixture.Customize<CreateInvoiceCommand>(c => c.FromFactory(() => new CreateInvoiceCommand(fixture.Create<CreateInvoiceRequest>())));
    }
}
