namespace BillLedger.Domain.Events;

public sealed record InvoiceCancelledDomainEvent(
    Guid InvoiceId,
    DateTime CancelledAt) : DomainEvent;
