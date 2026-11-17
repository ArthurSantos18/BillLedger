namespace BillLedger.IntegrationTests.Controllers;

public class CustomersControllerTests(WebApplicationFixture fixture) : ControllerTestBase(fixture)
{
    [Fact]
    public async Task Post_WithValidData_Returns201AndCustomer()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            Name = "Battler Integration",
            Email = $"integration-{Guid.NewGuid():N}@example.com",
            Cpf = GenerateRandomValidCpf()
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/customers", request, TestContext.Current.CancellationToken);

        // Assert
        var responseBody = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.Created, because: $"API Response: {responseBody}");

        var customer = await response.Content.ReadFromJsonAsync<CustomerResponse>(TestContext.Current.CancellationToken);
        customer.Should().NotBeNull();
        customer!.Name.Should().Be(request.Name);
        customer.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Post_WithInvalidData_Returns400BadRequest()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            Name = "",
            Email = "email-invalido",
            Cpf = "123"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/customers", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);
        problemDetails.Should().NotBeNull();
        problemDetails!.Errors.Should().ContainKeys("Name", "Email");
    }

    [Fact]
    public async Task Post_WithDuplicateCpf_Returns409Conflict()
    {
        // Arrange
        var customer = await CreateValidCustomerAsync();
        var request = new CreateCustomerRequest
        {
            Name = "Battler Clone",
            Email = $"clone-{Guid.NewGuid():N}@example.com",
            Cpf = customer.Cpf
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/customers", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Rename_WhenExists_Returns200Ok()
    {
        // Arrange
        var customer = await CreateValidCustomerAsync();
        var request = new RenameCustomerRequest { Name = "Battler Renomeado" };

        // Act
        var response = await Client.PatchAsJsonAsync($"/api/customers/{customer.Id}/rename", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var renamedCustomer = await response.Content.ReadFromJsonAsync<CustomerResponse>(TestContext.Current.CancellationToken);
        renamedCustomer!.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Rename_WhenNotExists_Returns404NotFound()
    {
        // Arrange
        var randomId = Guid.NewGuid();
        var request = new RenameCustomerRequest { Name = "Battler Renomeado" };

        // Act
        var response = await Client.PatchAsJsonAsync($"/api/customers/{randomId}/rename", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WhenExists_Returns204NoContent()
    {
        // Arrange
        var customer = await CreateValidCustomerAsync();

        // Act
        var response = await Client.DeleteAsync($"/api/customers/{customer.Id}", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_WhenNotExists_Returns404NotFound()
    {
        // Arrange
        var randomId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"/api/customers/{randomId}", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Returns200OkAndPagedResult()
    {
        // Act
        var response = await Client.GetAsync("/api/customers", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<CustomerResponse>>(TestContext.Current.CancellationToken);
        pagedResult.Should().NotBeNull();
        pagedResult!.Results.Should().NotBeNull();
    }

    private async Task<CustomerResponse> CreateValidCustomerAsync()
    {
        var request = new CreateCustomerRequest
        {
            Name = "Battler Auxiliar",
            Email = $"aux-{Guid.NewGuid():N}@example.com",
            Cpf = GenerateRandomValidCpf()
        };
        var response = await Client.PostAsJsonAsync("/api/customers", request, TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<CustomerResponse>(TestContext.Current.CancellationToken))!;
    }
}