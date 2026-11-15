namespace BillLedger.UnitTests.Application.Billing.Commands;

public sealed class CreateInvoiceCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<ILogger<CreateInvoiceCommandHandler>> _logger = new();
    private readonly CreateInvoiceCommandHandler _handler;

    public CreateInvoiceCommandHandlerTests()
    {
        _handler = new CreateInvoiceCommandHandler(
            _invoiceRepositoryMock.Object,
            _customerRepositoryMock.Object,
            _logger.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenCustomerExists_CreatesInvoiceAndReturnsResponse()
    {
        // Arrange
        var command = _fixture.Create<CreateInvoiceCommand>();
        var customer = _fixture.Create<Customer>();

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _invoiceRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Invoice>(), It.IsAny<CancellationToken>()), Times.Once);
        _invoiceRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenCustomerDoesNotExist_ReturnsCustomerNotFoundError()
    {
        // Arrange
        var command = _fixture.Create<CreateInvoiceCommand>();

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BillingErrors.CustomerNotFound);

        _invoiceRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Invoice>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}