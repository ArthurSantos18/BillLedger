namespace BillLedger.UnitTests.Application.Customers.Commands;

public sealed class RenameCustomerCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<ILogger<RenameCustomerCommandHandler>> _logger = new();
    private readonly RenameCustomerCommandHandler _handler;

    public RenameCustomerCommandHandlerTests()
    {
        _handler = new RenameCustomerCommandHandler(_customerRepositoryMock.Object, _logger.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenCustomerExists_RenamesCustomerAndReturnsResponse()
    {
        // Arrange
        var customer = _fixture.Create<Customer>();
        var command = new RenameCustomerCommand(customer.Id, _fixture.Create<RenameCustomerRequest>());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(customer.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _customerRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenCustomerDoesNotExist_ReturnsNotFoundError()
    {
        // Arrange
        var command = _fixture.Create<RenameCustomerCommand>();

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CustomerErrors.NotFound);

        _customerRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}