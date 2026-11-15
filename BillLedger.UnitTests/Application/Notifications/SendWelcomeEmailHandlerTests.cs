namespace BillLedger.UnitTests.Application.Notifications;

public class SendWelcomeEmailEventHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ILogger<SendWelcomeEmailEventHandler>> _logger = new();
    private readonly SendWelcomeEmailEventHandler _handler;

    public SendWelcomeEmailEventHandlerTests()
    {
        _handler = new SendWelcomeEmailEventHandler(_emailServiceMock.Object, _logger.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenEventIsReceived_SendsWelcomeEmail()
    {
        // Arrange
        var domainEvent = _fixture.Create<CustomerCreatedDomainEvent>();

        // Act
        await _handler.HandleAsync(domainEvent, CancellationToken.None);

        // Assert
        _emailServiceMock.Verify(x => x.SendEmailAsync(domainEvent.Email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}