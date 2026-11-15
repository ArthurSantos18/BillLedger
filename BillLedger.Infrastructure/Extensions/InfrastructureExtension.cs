namespace BillLedger.Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration.GetConnectionString("DefaultConnection")!);

        return services;
    }
}
