using BillLedger.Application.Extensions;

namespace BillLedger.API;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.AddBillLedgerLogging();

        builder.Services.AddBillLedgerOpenApi();
        builder.Services.AddFilters();
        builder.Services.AddMediator();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(configuration);
        builder.Services.AddBillLedgerRateLimiting();
        builder.Services.AddExceptionHandling();
        builder.Services.AddBillLedgerHealthChecks(configuration);

        var app = builder.Build();

        await app.SeedDatabaseAsync();

        app.UseBillLedgerForwardedHeaders();
        app.UseCorrelationIdMiddleware();
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseBillLedgerOpenApi();
        app.UseBillLedgerHealthChecks();
        app.UseBillLedgerRateLimiting();

        app.MapControllers();

        app.RunWithLogging();
    }
}