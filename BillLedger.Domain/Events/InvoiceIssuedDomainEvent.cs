namespace BillLedger.Domain.Events;

public sealed record InvoiceIssuedDomainEvent(
    Guid InvoiceId,
    Guid CustomerId,
    decimal TotalAmount,
    string CurrencyCode) : DomainEvent;
