namespace BillLedger.Application.Billing.Commands;

public sealed record CancelInvoiceCommand(Guid Id) : ICommand<Result>;
