namespace BillLedger.Application.Billing.Commands;

public sealed record CreateInvoiceCommand(CreateInvoiceRequest Request) : ICommand<Result<InvoiceResponse>>;