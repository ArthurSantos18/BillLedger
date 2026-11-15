namespace BillLedger.Application.Customers.Queries;

public sealed record GetCustomersQuery(GetCustomersRequest Request) : IQuery<PagedResult<CustomerResponse>>;
