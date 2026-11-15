namespace BillLedger.Application.Extensions;

public static class ValidatorExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateCustomerRequestValidator>();

        return services;
    }
}
