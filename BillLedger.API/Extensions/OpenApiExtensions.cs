namespace BillLedger.API.Extensions;

[ExcludeFromCodeCoverage(Justification = "Extension class for adding API services.")]
public static class OpenApiExtension
{
    public static IServiceCollection AddBillLedgerOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<DescriptionTransformer>();
            options.AddDocumentTransformer<TagDescriptionTransformer>();
        });

        return services;
    }

    public static WebApplication UseBillLedgerOpenApi(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options
            .WithTitle("BillLedger API")
            .WithTheme(ScalarTheme.Default);
        });

        return app;
    }
}
