namespace BillLedger.UnitTests.Application.Notifications;

public sealed class SendInvoiceIssuedEmailHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ILogger<SendInvoiceIssuedEmailEventHandler>> _logger = new();
    private readonly SendInvoiceIssuedEmailEventHandler _handler;

    public SendInvoiceIssuedEmailHandlerTests()
    {
        _handler = new SendInvoiceIssuedEmailEventHandler(_emailServiceMock.Object, _logger.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenEventIsReceived_SendsInvoiceIssuedEmail()
    {
        // Arrange
        var domainEvent = _fixture.Create<InvoiceIssuedDomainEvent>();

        // Act
        await _handler.HandleAsync(domainEvent, CancellationToken.None);

        // Assert
        _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), "Sua fatura foi emitida!", It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}