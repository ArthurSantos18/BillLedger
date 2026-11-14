namespace BillLedger.Domain.Exceptions;

public sealed class StateConflictException(string message) : DomainException(message);
