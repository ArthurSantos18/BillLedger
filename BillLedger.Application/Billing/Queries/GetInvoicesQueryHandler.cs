namespace BillLedger.Application.Billing.Queries;

public sealed class GetInvoicesQueryHandler(IInvoiceRepository repository) : IQueryHandler<GetInvoicesQuery, PagedResult<InvoiceResponse>>
{
    public async Task<PagedResult<InvoiceResponse>> HandleAsync(GetInvoicesQuery query, CancellationToken cancellationToken)
    {
        var pagedEntities = await repository.GetInvoicesAsync(query.Request, cancellationToken);

        return pagedEntities.Adapt<PagedResult<InvoiceResponse>>();
    }
}
