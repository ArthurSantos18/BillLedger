namespace BillLedger.Domain.Patterns;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}
