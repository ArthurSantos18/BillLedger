namespace BillLedger.UnitTests.Application.Customers.Queries;

public sealed class GetCustomersQueryHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly GetCustomersQueryHandler _handler;

    public GetCustomersQueryHandlerTests()
    {
        _handler = new GetCustomersQueryHandler(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WithValidRequest_ReturnsPagedResult()
    {
        // Arrange
        var request = _fixture.Create<GetCustomersRequest>();
        var query = new GetCustomersQuery(request);

        var customers = _fixture.CreateMany<Customer>(3).ToList();
        var pagedEntities = new PagedResult<Customer>(customers, 3, request.Page, request.PageSize);

        _customerRepositoryMock
            .Setup(x => x.GetCustomersAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedEntities);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Results.Should().HaveCount(3);
        result.TotalResults.Should().Be(3);

        _customerRepositoryMock.Verify(x => x.GetCustomersAsync(It.IsAny<GetCustomersRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}