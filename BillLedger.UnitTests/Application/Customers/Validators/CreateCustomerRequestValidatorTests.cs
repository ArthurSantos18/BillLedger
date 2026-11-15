namespace BillLedger.UnitTests.Application.Customers.Validators;

public sealed class CreateCustomerRequestValidatorTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly CreateCustomerRequestValidator _validator = new();

    [Fact]
    public void Validate_WithValidRequest_HasNoErrors()
    {
        // Arrange
        var request = _fixture.Create<CreateCustomerRequest>();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithEmptyName_HasError(string? invalidName)
    {
        // Arrange
        var request = _fixture.Create<CreateCustomerRequest>();
        request.Name = invalidName!;

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("O nome é obrigatório.");
    }

    [Fact]
    public void Validate_WithInvalidEmailFormat_HasError()
    {
        // Arrange
        var request = _fixture.Create<CreateCustomerRequest>();
        request.Email = "email-invalido";

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("O e-mail está em formato inválido.");
    }

    [Fact]
    public void Validate_WithNameTooLong_HasError()
    {
        // Arrange
        var request = _fixture.Create<CreateCustomerRequest>();
        request.Name = new string('a', 201);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("O nome deve ter no máximo 200 caracteres.");
    }
}