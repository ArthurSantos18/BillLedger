namespace BillLedger.Domain.Patterns;

public sealed class DomainException(string message) : Exception(message);
