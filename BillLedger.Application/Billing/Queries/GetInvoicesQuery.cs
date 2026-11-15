namespace BillLedger.Application.Billing.Queries;

public sealed record GetInvoicesQuery(GetInvoicesRequest Request) : IQuery<PagedResult<InvoiceResponse>>;
