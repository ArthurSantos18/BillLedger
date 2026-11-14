namespace BillLedger.Domain.Events;

public sealed record CustomerCreatedDomainEvent(
    Guid CustomerId,
    string Email,
    string Cpf) : DomainEvent;
