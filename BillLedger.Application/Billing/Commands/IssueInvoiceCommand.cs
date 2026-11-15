namespace BillLedger.Application.Billing.Commands;

public sealed record IssueInvoiceCommand(Guid Id) : ICommand<Result<InvoiceResponse>>;
