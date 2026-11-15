namespace BillLedger.Application.Billing.Commands;

public sealed record PayInvoiceCommand(Guid Id, PayInvoiceRequest Request) : ICommand<Result>;
