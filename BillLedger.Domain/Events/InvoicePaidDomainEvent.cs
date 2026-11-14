namespace BillLedger.Domain.Events;

public sealed record InvoicePaidDomainEvent(
    Guid InvoiceId,
    Guid CustomerId,
    decimal TotalAmount,
    string CurrencyCode,
    DateTime PaidAt) : DomainEvent;
