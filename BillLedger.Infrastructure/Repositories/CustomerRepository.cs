namespace BillLedger.Infrastructure.Repositories;

public sealed class CustomerRepository(AppDbContext context) : BaseRepository<Customer>(context), ICustomerRepository
{
    public async Task<Customer?> GetByCpfAsync(Cpf cpf, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Cpf == cpf, cancellationToken);
    }

    public async Task<Customer?> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public async Task<PagedResult<Customer>> GetCustomersAsync(GetCustomersRequest request, CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        query = ApplyNameFilter(query, request.Name);
        query = ApplyEmailFilter(query, request.Email);
        query = ApplyCpfFilter(query, request.Cpf);
        query = ApplyIsActiveFilter(query, request.IsActive);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip(request.Skip)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Customer>(items, total, request.Page, request.PageSize);
    }

    private static IQueryable<Customer> ApplyNameFilter(IQueryable<Customer> query, string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            return query.Where(c => c.Name.Contains(name));
        }

        return query;
    }

    private static IQueryable<Customer> ApplyEmailFilter(IQueryable<Customer> query, string? email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            return query.Where(c => EF.Functions.ILike(EF.Property<string>(c, "Email"), email));
        }

        return query;
    }

    private static IQueryable<Customer> ApplyCpfFilter(IQueryable<Customer> query, string? cpf)
    {
        if (!string.IsNullOrWhiteSpace(cpf))
        {
            return query.Where(c => EF.Property<string>(c, "Cpf") == cpf);
        }

        return query;
    }

    private static IQueryable<Customer> ApplyIsActiveFilter(IQueryable<Customer> query, bool? isActive)
    {
        if (isActive.HasValue)
        {
            return query.Where(c => c.IsActive == isActive.Value);
        }

        return query;
    }
}
