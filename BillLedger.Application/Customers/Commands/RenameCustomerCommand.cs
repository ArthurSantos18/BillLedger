namespace BillLedger.Application.Customers.Commands;

public sealed record RenameCustomerCommand(Guid Id, RenameCustomerRequest Request) : ICommand<Result<CustomerResponse>>;
