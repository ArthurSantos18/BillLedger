namespace BillLedger.Application.Customers.Responses;

public sealed record CustomerResponse(
    Guid Id,
    string Name,
    string Email,
    string Cpf,
    bool IsActive,
    DateTime CreatedAt);
