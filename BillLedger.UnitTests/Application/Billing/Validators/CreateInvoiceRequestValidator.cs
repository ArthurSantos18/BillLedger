namespace BillLedger.UnitTests.Application.Billing.Validators;

public sealed class CreateInvoiceRequestValidatorTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly CreateInvoiceRequestValidator _validator = new();

    [Fact]
    public void Validate_WithValidRequest_HasNoErrors()
    {
        // Arrange
        var request = _fixture.Create<CreateInvoiceRequest>();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithZeroAmount_HasError()
    {
        // Arrange
        var request = _fixture.Create<CreateInvoiceRequest>();
        request.Amount = 0;

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Amount).WithErrorMessage("O valor da fatura deve ser maior que zero.");
    }

    [Fact]
    public void Validate_WithPastDueDate_HasError()
    {
        // Arrange
        var request = _fixture.Create<CreateInvoiceRequest>();
        request.DueDate = DateTime.UtcNow.AddDays(-1);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DueDate).WithErrorMessage("A data de vencimento não pode ser no passado.");
    }
}