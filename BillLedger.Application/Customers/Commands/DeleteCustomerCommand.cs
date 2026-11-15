namespace BillLedger.Application.Customers.Commands;

public sealed record DeleteCustomerCommand(Guid Id) : ICommand<Result>;
