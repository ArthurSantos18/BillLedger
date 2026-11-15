namespace BillLedger.Application.Customers.Commands;

public sealed record CreateCustomerCommand(CreateCustomerRequest Request) : ICommand<Result<CustomerResponse>>;
