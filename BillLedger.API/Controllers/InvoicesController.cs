namespace BillLedger.API.Controllers;

[ApiController]
[Route("api/invoices")]
[ApiConventionType(typeof(InvoicesMetadata))]
public class InvoicesController(ICommandMediator commandMediator, IQueryMediator queryMediator) : ControllerBase
{
    [HttpPost]
    [Stability(Stability.Stable)]
    [EndpointDescription("Cadastra uma nova fatura com as informações fornecidas.")]
    public async Task<IActionResult> CreateInvoiceAsync([FromBody] CreateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var result = await commandMediator.SendAsync(new CreateInvoiceCommand(request), cancellationToken);

        return result.IsSuccess
            ? Created(string.Empty, result.Value)
            : StatusCode(result.Error.Code, result.Error);
    }

    [HttpPost("{id:guid}/issue")]
    [Stability(Stability.Stable)]
    [EndpointDescription("Emite uma fatura existente identificada pelo seu ID.")]
    public async Task<IActionResult> IssueInvoiceAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await commandMediator.SendAsync(new IssueInvoiceCommand(id), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : StatusCode(result.Error.Code, result.Error);
    }

    [HttpPost("{id:guid}/pay")]
    [Stability(Stability.Stable)]
    [EndpointDescription("Registra o pagamento de uma fatura existente identificada pelo seu ID.")]
    public async Task<IActionResult> PayInvoiceAsync(Guid id, [FromBody] PayInvoiceRequest request, CancellationToken cancellationToken)
    {
        var result = await commandMediator.SendAsync(new PayInvoiceCommand(id, request), cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : StatusCode(result.Error.Code, result.Error);
    }

    [HttpPost("{id:guid}/cancel")]
    [Stability(Stability.Stable)]
    [EndpointDescription("Cancela uma fatura existente identificada pelo seu ID.")]
    public async Task<IActionResult> CancelInvoiceAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await commandMediator.SendAsync(new CancelInvoiceCommand(id), cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : StatusCode(result.Error.Code, result.Error);
    }

    [HttpGet]
    [Stability(Stability.Stable)]
    [EndpointDescription("Consulta a lista de faturas, permitindo paginação e aplicação de filtros.")]
    public async Task<IActionResult> GetInvoicesAsync([FromQuery] GetInvoicesRequest request, CancellationToken cancellationToken)
    {
        var result = await queryMediator.QueryAsync(new GetInvoicesQuery(request), cancellationToken);

        return Ok(result);
    }
}