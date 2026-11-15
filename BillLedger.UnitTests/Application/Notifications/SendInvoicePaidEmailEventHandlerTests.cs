namespace BillLedger.UnitTests.Application.Notifications;

public sealed class SendInvoicePaidEmailHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ILogger<SendInvoicePaidEmailEventHandler>> _logger = new();
    private readonly SendInvoicePaidEmailEventHandler _handler;

    public SendInvoicePaidEmailHandlerTests()
    {
        _handler = new SendInvoicePaidEmailEventHandler(_emailServiceMock.Object, _logger.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenEventIsReceived_SendsInvoicePaidEmail()
    {
        // Arrange
        var domainEvent = _fixture.Create<InvoicePaidDomainEvent>();

        // Act
        await _handler.HandleAsync(domainEvent, CancellationToken.None);

        // Assert
        _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), "Pagamento Confirmado!", It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}