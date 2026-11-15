namespace BillLedger.UnitTests.Application.Billing.Queries;

public sealed class GetInvoicesQueryHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());
    private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock = new();
    private readonly GetInvoicesQueryHandler _handler;

    public GetInvoicesQueryHandlerTests()
    {
        _handler = new GetInvoicesQueryHandler(_invoiceRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WithValidRequest_ReturnsPagedResult()
    {
        // Arrange
        var request = _fixture.Create<GetInvoicesRequest>();
        var query = new GetInvoicesQuery(request);

        var invoices = _fixture.CreateMany<Invoice>(3).ToList();
        var pagedEntities = new PagedResult<Invoice>(invoices, 3, request.Page, request.PageSize);

        _invoiceRepositoryMock
            .Setup(x => x.GetInvoicesAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedEntities);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Results.Should().HaveCount(3);
        result.TotalResults.Should().Be(3);

        _invoiceRepositoryMock.Verify(x => x.GetInvoicesAsync(It.IsAny<GetInvoicesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}