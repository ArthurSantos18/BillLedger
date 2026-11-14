namespace BillLedger.UnitTests.Domain.Entities;

public class CustomerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());

    [Fact]
    public void Create_WithValidData_ReturnsActiveCustomer()
    {
        // Arrange
        var name = "Ushiromiya Battler";
        var email = _fixture.Create<Email>();
        var cpf = _fixture.Create<Cpf>();

        // Act
        var customer = Customer.Create(name, email, cpf);

        // Assert
        customer.Id.Should().NotBe(Guid.Empty);
        customer.Name.Should().Be(name);
        customer.Email.Should().Be(email);
        customer.Cpf.Should().Be(cpf);
        customer.IsActive.Should().BeTrue();
        customer.IsDeleted.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyName_ThrowsDomainException(string? invalidName)
    {
        // Arrange
        var email = _fixture.Create<Email>();
        var cpf = _fixture.Create<Cpf>();

        // Act
        var act = () => Customer.Create(invalidName!, email, cpf);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*obrigatório*");
    }

    [Fact]
    public void Create_WithNameTooLong_ThrowsDomainException()
    {
        // Arrange
        var longName = new string('a', 201);
        var email = _fixture.Create<Email>();
        var cpf = _fixture.Create<Cpf>();

        // Act
        var act = () => Customer.Create(longName, email, cpf);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*200 caracteres*");
    }

    [Fact]
    public void Create_WithNullEmail_ThrowsDomainException()
    {
        // Arrange
        var cpf = Cpf.Create("12345678909");

        // Act
        var act = () => Customer.Create("Battler", null!, cpf);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*E-mail*obrigatório*");
    }

    [Fact]
    public void Create_WithNullCpf_ThrowsDomainException()
    {
        // Arrange
        var email = Email.Create("user@example.com");

        // Act
        var act = () => Customer.Create("Battler", email, null!);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*CPF*obrigatório*");
    }

    [Fact]
    public void Create_RaisesCustomerCreatedDomainEvent()
    {
        // Arrange
        var email = _fixture.Create<Email>();
        var cpf = _fixture.Create<Cpf>();

        // Act
        var customer = Customer.Create("Battler", email, cpf);

        // Assert
        customer.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<CustomerCreatedDomainEvent>();
    }

    [Fact]
    public void Rename_WithValidName_UpdatesName()
    {
        // Arrange
        var customer = Customer.Create("Battler", _fixture.Create<Email>(), _fixture.Create<Cpf>());

        // Act
        customer.Rename("Ushiromiya Battler");

        // Assert
        customer.Name.Should().Be("Ushiromiya Battler");
    }

    [Fact]
    public void Rename_WithEmptyName_ThrowsDomainException()
    {
        // Arrange
        var customer = Customer.Create("Battler", _fixture.Create<Email>(), _fixture.Create<Cpf>());

        // Act
        var act = () => customer.Rename("   ");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*obrigatório*");
    }

    [Fact]
    public void ChangeEmail_WithNewEmail_UpdatesEmail()
    {
        // Arrange
        var customer = Customer.Create("Battler", _fixture.Create<Email>(), _fixture.Create<Cpf>());
        var newEmail = Email.Create("other@example.com");

        // Act
        customer.ChangeEmail(newEmail);

        // Assert
        customer.Email.Should().Be(newEmail);
    }
}
