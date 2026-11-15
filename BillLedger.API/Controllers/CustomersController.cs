namespace BillLedger.API.Controllers;

[ApiController]
[Route("api/customers")]
[ApiConventionType(typeof(CustomersMetadata))]
public class CustomersController(ICommandMediator commandMediator, IQueryMediator queryMediator) : ControllerBase
{
    [HttpPost]
    [Stability(Stability.Stable)]
    [EndpointDescription("Cadastra um novo cliente com as informações fornecidas.")]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await commandMediator.SendAsync(new CreateCustomerCommand(request), cancellationToken);

        return result.IsSuccess
            ? Created(string.Empty, result.Value)
            : StatusCode(result.Error.Code, result.Error);
    }

    [HttpPatch("{id:guid}/rename")]
    [Stability(Stability.Stable)]
    [EndpointDescription("Altera o nome de um cliente existente identificado pelo seu ID.")]
    public async Task<IActionResult> RenameCustomerAsync(Guid id, [FromBody] RenameCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await commandMediator.SendAsync(new RenameCustomerCommand(id, request), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : StatusCode(result.Error.Code, result.Error);
    }

    [HttpDelete("{id:guid}")]
    [Stability(Stability.Stable)]
    [EndpointDescription("Remove um cliente existente identificado pelo seu ID.")]
    public async Task<IActionResult> DeleteCustomerAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await commandMediator.SendAsync(new DeleteCustomerCommand(id), cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : StatusCode(result.Error.Code, result.Error);
    }

    [HttpGet]
    [Stability(Stability.Stable)]
    [EndpointDescription("Consulta a lista de clientes, permitindo paginação e aplicação de filtros.")]
    public async Task<IActionResult> GetCustomersAsync([FromQuery] GetCustomersRequest request, CancellationToken cancellationToken)
    {
        var result = await queryMediator.QueryAsync(new GetCustomersQuery(request), cancellationToken);

        return Ok(result);
    }
}
