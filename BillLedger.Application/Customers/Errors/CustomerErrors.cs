namespace BillLedger.Application.Customers.Errors;

public static class CustomerErrors
{
    public static readonly Error NotFound = new(404, "Cliente não encontrado.");
    public static readonly Error CpfAlreadyInUse = new(409, "Já existe um cliente cadastrado com este CPF.");
    public static readonly Error EmailAlreadyInUse = new(409, "Já existe um cliente cadastrado com este e-mail.");
}
