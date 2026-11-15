namespace BillLedger.Infrastructure.Repositories;

public sealed class InvoiceRepository(AppDbContext context) : BaseRepository<Invoice>(context), IInvoiceRepository
{
    public async Task<PagedResult<Invoice>> GetInvoicesAsync(GetInvoicesRequest request, CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        query = ApplyCustomerFilter(query, request.CustomerId);
        query = ApplyStatusFilter(query, request.Status);
        query = ApplyDueDateFilter(query, request.MinDueDate, request.MaxDueDate);
        query = ApplyTotalAmountFilter(query, request.MinTotalAmount, request.MaxTotalAmount);
        query = ApplyCreatedAtFilter(query, request.MinCreatedAt, request.MaxCreatedAt);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip(request.Skip)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Invoice>(items, total, request.Page, request.PageSize);
    }

    private static IQueryable<Invoice> ApplyCustomerFilter(IQueryable<Invoice> query, Guid? customerId)
    {
        if (customerId.HasValue && customerId.Value != Guid.Empty)
        {
            return query.Where(i => i.CustomerId == customerId.Value);
        }

        return query;
    }

    private static IQueryable<Invoice> ApplyStatusFilter(IQueryable<Invoice> query, InvoiceStatus? status)
    {
        if (status.HasValue)
        {
            return query.Where(i => i.Status == status.Value);

        }

        return query;
    }

    private static IQueryable<Invoice> ApplyDueDateFilter(IQueryable<Invoice> query, DateTime? min, DateTime? max)
    {
        if (min.HasValue)
        {
            query = query.Where(i => i.DueDate >= min.Value.Date);
        }

        if (max.HasValue)
        {
            query = query.Where(i => i.DueDate <= max.Value.Date.AddDays(1).AddSeconds(-1));
        }

        return query;
    }

    private static IQueryable<Invoice> ApplyTotalAmountFilter(IQueryable<Invoice> query, decimal? min, decimal? max)
    {
        if (min.HasValue)
        {
            query = query.Where(i => i.Total.Amount >= min.Value);
        }

        if (max.HasValue)
        {
            query = query.Where(i => i.Total.Amount <= max.Value);
        }

        return query;
    }

    private static IQueryable<Invoice> ApplyCreatedAtFilter(IQueryable<Invoice> query, DateTime? min, DateTime? max)
    {
        if (min.HasValue)
        {
            query = query.Where(i => i.CreatedAt >= min.Value.Date);
        }

        if (max.HasValue)
        {
            query = query.Where(i => i.CreatedAt <= max.Value.Date.AddDays(1).AddSeconds(-1));
        }

        return query;
    }
}
