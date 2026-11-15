namespace BillLedger.Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();

        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")!);

            var auditInterceptor = serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>();
            var dispatchInterceptor = serviceProvider.GetRequiredService<DispatchDomainEventsInterceptor>();

            options.AddInterceptors(auditInterceptor, dispatchInterceptor);
        });

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
