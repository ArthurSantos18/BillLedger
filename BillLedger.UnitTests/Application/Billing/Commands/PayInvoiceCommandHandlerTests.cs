namespace BillLedger.UnitTests.Application.Billing.Commands;

public sealed class PayInvoiceCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock = new();
    private readonly Mock<ILogger<PayInvoiceCommandHandler>> _logger = new();
    private readonly PayInvoiceCommandHandler _handler;

    public PayInvoiceCommandHandlerTests()
    {
        _handler = new PayInvoiceCommandHandler(_invoiceRepositoryMock.Object, _logger.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoiceIsIssued_PaysInvoiceAndReturnsSuccess()
    {
        // Arrange
        var invoice = _fixture.Create<Invoice>();
        invoice.Issue();
        var command = new PayInvoiceCommand(invoice.Id, _fixture.Create<PayInvoiceRequest>());

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
    public async Task HandleAsync_WhenInvoiceIsAlreadyPaid_ThrowsStateConflictException()
    {
        // Arrange
        var invoice = _fixture.Create<Invoice>();
        invoice.Issue();
        invoice.MarkAsPaid();
        var command = new PayInvoiceCommand(invoice.Id, _fixture.Create<PayInvoiceRequest>());

        _invoiceRepositoryMock
            .Setup(x => x.GetByIdAsync(invoice.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);

        // Act
        var act = () => _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<StateConflictException>().WithMessage("*já está paga*");
    }
}