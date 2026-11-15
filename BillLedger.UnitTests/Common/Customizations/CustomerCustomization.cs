namespace BillLedger.UnitTests.Common.Customizations;

public class CustomerCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<CreateCustomerRequest>(c => c
            .With(r => r.Name, fixture.Create<string>())
            .With(r => r.Email, () => fixture.Create<Email>().Value)
            .With(r => r.Cpf, () => fixture.Create<Cpf>().Value));

        fixture.Customize<RenameCustomerRequest>(c => c
            .With(r => r.Name, () => fixture.Create<string>()));

        fixture.Customize<CreateCustomerCommand>(c => c.FromFactory(() => new CreateCustomerCommand(fixture.Create<CreateCustomerRequest>())));
        fixture.Customize<RenameCustomerCommand>(c => c.FromFactory(() => new RenameCustomerCommand(Guid.NewGuid(), fixture.Create<RenameCustomerRequest>())));
    }
}
