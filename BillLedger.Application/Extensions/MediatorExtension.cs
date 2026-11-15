namespace BillLedger.Application.Extensions;

public static class MediatorExtension
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddLiteBus(liteBus =>
        {
            liteBus.AddMessaging(_ => { });
            liteBus.AddCommands(module =>
            {
                module.RegisterFromAssembly(typeof(CreateCustomerCommand).Assembly);
            });

            liteBus.AddQueries(module =>
            {
                module.RegisterFromAssembly(typeof(GetCustomersQuery).Assembly);
            });
            liteBus.AddEvents(module =>
            {
                module.RegisterFromAssembly(typeof(SendWelcomeEmailEventHandler).Assembly);
            });
        });

        return services;
    }
}
