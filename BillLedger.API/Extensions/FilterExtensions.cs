using BillLedger.API.Filters;

namespace BillLedger.API.Extensions;

public static class FilterExtensions
{
    public static IServiceCollection AddFilters(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ResultToProblemDetailsFilter>();
        });

        return services;
    }
}