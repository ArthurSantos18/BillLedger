namespace BillLedger.UnitTests.Application.Billing.Commands;

public sealed class CancelInvoiceCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock = new();
    private readonly Mock<ILogger<CancelInvoiceCommandHandler>> _logger = new();
    private readonly CancelInvoiceCommandHandler _handler;

    public CancelInvoiceCommandHandlerTests()
    {
        _handler = new CancelInvoiceCommandHandler(_invoiceRepositoryMock.Object, _logger.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoiceIsDraft_CancelsInvoiceAndReturnsSuccess()
    {
        // Arrange
        var invoice = _fixture.Create<Invoice>();
        var command = new CancelInvoiceCommand(invoice.Id); 

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
    public async Task HandleAsync_WhenInvoiceIsPaid_ThrowsStateConflictException()
    {
        // Arrange
        var invoice = _fixture.Create<Invoice>();
        invoice.Issue();
        invoice.MarkAsPaid();
        var command = new CancelInvoiceCommand(invoice.Id);

        _invoiceRepositoryMock
            .Setup(x => x.GetByIdAsync(invoice.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);

        // Act
        var act = () => _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<StateConflictException>().WithMessage("*paga*não pode ser cancelada*");

        _invoiceRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}