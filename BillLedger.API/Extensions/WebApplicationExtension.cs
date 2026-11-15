namespace BillLedger.API.Extensions;

[ExcludeFromCodeCoverage(Justification = "Extension class for adding API services.")]
public static class WebApplicationExtension
{
    public static void RunWithLogging(this WebApplication app)
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            Log.Fatal(args.ExceptionObject as Exception, "Unhandled exception on AppDomain.");
            Log.CloseAndFlush();
        };

        TaskScheduler.UnobservedTaskException += (sender, args) =>
        {
            Log.Error(args.Exception, "Unobserved task exception.");
        };

        app.Lifetime.ApplicationStarted.Register(() =>
            Log.Information("BillLedger API started successfully."));

        app.Lifetime.ApplicationStopping.Register(() =>
            Log.Information("BillLedger API is shutting down..."));

        app.Lifetime.ApplicationStopped.Register(() =>
        {
            Log.Information("BillLedger API stopped.");
            Log.CloseAndFlush();
        });

        try
        {
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "BillLedger API terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
