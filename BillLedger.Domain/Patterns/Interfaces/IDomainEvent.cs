namespace BillLedger.Domain.Patterns.Interfaces;

public interface IDomainEvent : IEvent
{
    DateTime OccurredAt { get; }
}
