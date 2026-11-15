namespace BillLedger.Domain.Patterns.Interfaces;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}
