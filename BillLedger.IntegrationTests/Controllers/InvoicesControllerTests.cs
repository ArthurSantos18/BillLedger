namespace BillLedger.IntegrationTests.Controllers;

public class InvoicesControllerTests(WebApplicationFixture fixture) : ControllerTestBase(fixture)
{
    [Fact]
    public async Task Create_WithValidData_Returns201AndDraftInvoice()
    {
        // Arrange
        var customer = await CreateValidCustomerAsync();
        var request = new CreateInvoiceRequest
        {
            CustomerId = customer.Id,
            Number = $"INV-{Guid.NewGuid():N}",
            Amount = 150.50m,
            CurrencyCode = "BRL",
            DueDate = DateTime.UtcNow.AddDays(10)
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/invoices", request, TestContext.Current.CancellationToken);

        // Assert
        var responseBody = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.Created, because: $"API Response: {responseBody}");

        var invoice = await response.Content.ReadFromJsonAsync<InvoiceResponse>(TestContext.Current.CancellationToken);
        invoice.Should().NotBeNull();
        invoice!.Status.Should().Be(InvoiceStatus.Draft);
        invoice.Amount.Should().Be(request.Amount);
    }

    [Fact]
    public async Task Create_WithInvalidCustomerId_Returns404NotFound()
    {
        // Arrange
        var request = new CreateInvoiceRequest
        {
            CustomerId = Guid.NewGuid(),
            Number = $"INV-{Guid.NewGuid():N}",
            Amount = 100m,
            CurrencyCode = "BRL",
            DueDate = DateTime.UtcNow.AddDays(10)
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/invoices", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Issue_WhenDraft_Returns200OkAndIssuedStatus()
    {
        // Arrange
        var invoice = await CreateValidInvoiceAsync();

        // Act
        var response = await Client.PostAsync($"/api/invoices/{invoice.Id}/issue", null, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var issuedInvoice = await response.Content.ReadFromJsonAsync<InvoiceResponse>(TestContext.Current.CancellationToken);
        issuedInvoice!.Status.Should().Be(InvoiceStatus.Issued);
    }

    [Fact]
    public async Task Issue_WhenAlreadyIssued_Returns409Conflict()
    {
        // Arrange
        var invoice = await CreateValidInvoiceAsync();
        await Client.PostAsync($"/api/invoices/{invoice.Id}/issue", null, TestContext.Current.CancellationToken); // Emite a primeira vez

        // Act
        var response = await Client.PostAsync($"/api/invoices/{invoice.Id}/issue", null, TestContext.Current.CancellationToken); // Tenta emitir de novo

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);
        problemDetails!.Detail.Should().Contain("rascunho");
    }

    [Fact]
    public async Task Issue_WhenNotExists_Returns404NotFound()
    {
        // Act
        var response = await Client.PostAsync($"/api/invoices/{Guid.NewGuid()}/issue", null, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Pay_WhenIssued_Returns204NoContent()
    {
        // Arrange
        var invoice = await CreateValidInvoiceAsync();
        await Client.PostAsync($"/api/invoices/{invoice.Id}/issue", null, TestContext.Current.CancellationToken);
        var request = new PayInvoiceRequest { PaidAt = DateTime.UtcNow };

        // Act
        var response = await Client.PostAsJsonAsync($"/api/invoices/{invoice.Id}/pay", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Pay_WhenAlreadyPaid_Returns409Conflict()
    {
        // Arrange
        var invoice = await CreateValidInvoiceAsync();
        await Client.PostAsync($"/api/invoices/{invoice.Id}/issue", null, TestContext.Current.CancellationToken);
        var request = new PayInvoiceRequest { PaidAt = DateTime.UtcNow };
        await Client.PostAsJsonAsync($"/api/invoices/{invoice.Id}/pay", request, TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PostAsJsonAsync($"/api/invoices/{invoice.Id}/pay", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Pay_WhenDraft_Returns409Conflict()
    {
        // Arrange
        var invoice = await CreateValidInvoiceAsync();
        var request = new PayInvoiceRequest { PaidAt = DateTime.UtcNow };

        // Act
        var response = await Client.PostAsJsonAsync($"/api/invoices/{invoice.Id}/pay", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Pay_WhenNotExists_Returns404NotFound()
    {
        // Arrange
        var request = new PayInvoiceRequest { PaidAt = DateTime.UtcNow };

        // Act
        var response = await Client.PostAsJsonAsync($"/api/invoices/{Guid.NewGuid()}/pay", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cancel_WhenDraft_Returns204NoContent()
    {
        // Arrange
        var invoice = await CreateValidInvoiceAsync();

        // Act
        var response = await Client.PostAsync($"/api/invoices/{invoice.Id}/cancel", null, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Cancel_WhenPaid_Returns409Conflict()
    {
        // Arrange
        var invoice = await CreateValidInvoiceAsync();
        await Client.PostAsync($"/api/invoices/{invoice.Id}/issue", null, TestContext.Current.CancellationToken);
        var request = new PayInvoiceRequest { PaidAt = DateTime.UtcNow };
        await Client.PostAsJsonAsync($"/api/invoices/{invoice.Id}/pay", request, TestContext.Current.CancellationToken);

        // Act
        var response = await Client.PostAsync($"/api/invoices/{invoice.Id}/cancel", null, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Get_Returns200OkAndPagedResult()
    {
        // Act
        var response = await Client.GetAsync("/api/invoices", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<InvoiceResponse>>(TestContext.Current.CancellationToken);
        pagedResult.Should().NotBeNull();
        pagedResult!.Results.Should().NotBeNull();
    }

    private async Task<CustomerResponse> CreateValidCustomerAsync()
    {
        var request = new CreateCustomerRequest
        {
            Name = "Battler Fatura",
            Email = $"battler-{Guid.NewGuid():N}@example.com",
            Cpf = GenerateRandomValidCpf()
        };
        var response = await Client.PostAsJsonAsync("/api/customers", request, TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<CustomerResponse>(TestContext.Current.CancellationToken))!;
    }

    private async Task<InvoiceResponse> CreateValidInvoiceAsync()
    {
        var customer = await CreateValidCustomerAsync();
        var request = new CreateInvoiceRequest
        {
            CustomerId = customer.Id,
            Number = $"INV-{Guid.NewGuid():N}",
            Amount = 100m,
            CurrencyCode = "BRL",
            DueDate = DateTime.UtcNow.AddDays(5)
        };
        var response = await Client.PostAsJsonAsync("/api/invoices", request, TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<InvoiceResponse>(TestContext.Current.CancellationToken))!;
    }
}