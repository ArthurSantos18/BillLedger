namespace BillLedger.UnitTests.Application.Notifications;

public class SendInvoiceCancelledEmailHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ILogger<SendInvoiceCancelledEmailEventHandler>> _logger = new();
    private readonly SendInvoiceCancelledEmailEventHandler _handler;

    public SendInvoiceCancelledEmailHandlerTests()
    {
        _handler = new SendInvoiceCancelledEmailEventHandler(_emailServiceMock.Object, _logger.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenEventIsReceived_SendsInvoiceCancelledEmail()
    {
        // Arrange
        var domainEvent = _fixture.Create<InvoiceCancelledDomainEvent>();

        // Act
        await _handler.HandleAsync(domainEvent, CancellationToken.None);

        // Assert
        _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), "Fatura Cancelada", It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}