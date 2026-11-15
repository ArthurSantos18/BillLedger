namespace BillLedger.UnitTests.Application.Customers.Validators;

public class RenameCustomerRequestValidatorTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly RenameCustomerRequestValidator _validator = new();

    [Fact]
    public void Validate_WithValidRequest_HasNoErrors()
    {
        // Arrange
        var request = _fixture.Create<RenameCustomerRequest>();

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
        var request = _fixture.Create<RenameCustomerRequest>();
        request.Name = invalidName!;

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("O nome é obrigatório.");
    }

    [Fact]
    public void Validate_WithNameTooLong_HasError()
    {
        // Arrange
        var request = _fixture.Create<RenameCustomerRequest>();
        request.Name = new string('a', 201);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("O nome deve ter no máximo 200 caracteres.");
    }
}
