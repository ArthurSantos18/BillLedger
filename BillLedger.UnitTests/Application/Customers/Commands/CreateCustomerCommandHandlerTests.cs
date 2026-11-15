namespace BillLedger.UnitTests.Application.Customers.Commands;

public sealed class CreateCustomerCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<ILogger<CreateCustomerCommandHandler>> _logger = new();
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, _logger.Object);
    }

    [Fact]
    public async Task HandleAsync_WithValidData_CreatesCustomerAndReturnsResponse()
    {
        // Arrange
        var command = _fixture.Create<CreateCustomerCommand>();

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(It.IsAny<Cpf>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        _customerRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _customerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        _customerRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenCpfAlreadyExists_ReturnsCpfAlreadyInUseError()
    {
        // Arrange
        var command = _fixture.Create<CreateCustomerCommand>();
        var existingCustomer = _fixture.Create<Customer>();

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(It.IsAny<Cpf>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCustomer);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CustomerErrors.CpfAlreadyInUse);

        _customerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
        _customerRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WhenEmailAlreadyExists_ReturnsEmailAlreadyInUseError()
    {
        // Arrange
        var command = _fixture.Create<CreateCustomerCommand>();
        var existingCustomer = _fixture.Create<Customer>();

        _customerRepositoryMock
            .Setup(x => x.GetByCpfAsync(It.IsAny<Cpf>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        _customerRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCustomer);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CustomerErrors.EmailAlreadyInUse);

        _customerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
        _customerRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}