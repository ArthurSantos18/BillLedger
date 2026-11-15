namespace BillLedger.Application.Common.Interfaces.Repositories;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer?> GetByCpfAsync(Cpf cpf, CancellationToken cancellationToken);
    Task<Customer?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
    Task<PagedResult<Customer>> GetCustomersAsync(GetCustomersRequest request, CancellationToken cancellationToken);
}
