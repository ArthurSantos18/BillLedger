namespace BillLedger.UnitTests.Application.Billing.Commands;

public sealed class IssueInvoiceCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock = new();
    private readonly Mock<ILogger<IssueInvoiceCommandHandler>> _logger = new();
    private readonly IssueInvoiceCommandHandler _handler;

    public IssueInvoiceCommandHandlerTests()
    {
        _handler = new IssueInvoiceCommandHandler(_invoiceRepositoryMock.Object, _logger.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoiceIsDraft_IssuesInvoiceAndReturnsSuccess()
    {
        // Arrange
        var invoice = _fixture.Create<Invoice>();
        var command = new IssueInvoiceCommand(invoice.Id);

        _invoiceRepositoryMock
            .Setup(x => x.GetByIdAsync(invoice.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _invoiceRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoiceDoesNotExist_ReturnsNotFoundError()
    {
        // Arrange
        var command = new IssueInvoiceCommand(Guid.NewGuid());

        _invoiceRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Invoice?)null);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BillingErrors.NotFound);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoiceIsAlreadyIssued_ThrowsStateConflictException()
    {
        // Arrange
        var invoice = _fixture.Create<Invoice>();
        invoice.Issue();
        var command = new IssueInvoiceCommand(invoice.Id);

        _invoiceRepositoryMock
            .Setup(x => x.GetByIdAsync(invoice.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);

        // Act
        var act = () => _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<StateConflictException>().WithMessage("*rascunho*");

        _invoiceRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}