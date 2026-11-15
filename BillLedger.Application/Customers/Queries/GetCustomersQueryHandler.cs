namespace BillLedger.Application.Customers.Queries;

public sealed class GetCustomersQueryHandler(ICustomerRepository repository) : IQueryHandler<GetCustomersQuery, PagedResult<CustomerResponse>>
{
    public async Task<PagedResult<CustomerResponse>> HandleAsync(GetCustomersQuery query, CancellationToken cancellationToken)
    {
        var pagedEntities = await repository.GetCustomersAsync(query.Request, cancellationToken);

        return pagedEntities.Adapt<PagedResult<CustomerResponse>>();
    }
}
