namespace BillLedger.Application.Billing.Responses;

public sealed record InvoiceResponse(
    Guid Id,
    Guid CustomerId,
    string Number,
    decimal Amount,
    string Currency,
    InvoiceStatus Status,
    DateTime DueDate,
    DateTime? PaidAt,
    DateTime? CancelledAt);
