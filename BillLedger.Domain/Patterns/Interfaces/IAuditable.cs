namespace BillLedger.Domain.Patterns.Interfaces;

public interface IAuditable
{
    void SetCreatedAt(DateTime date);
    void SetUpdatedAt(DateTime date);
}
