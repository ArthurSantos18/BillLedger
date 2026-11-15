namespace BillLedger.Application.Common.Interfaces.Repositories;

public interface IInvoiceRepository : IBaseRepository<Invoice>
{
    Task<PagedResult<Invoice>> GetInvoicesAsync(GetInvoicesRequest request, CancellationToken cancellationToken);
}
