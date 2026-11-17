namespace BillLedger.IntegrationTests.Base;

public class WebApplicationFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private IConfiguration? _configuration;
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("BillLedgerTestDb")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public async ValueTask InitializeAsync()
    {
        MappingConfig.RegisterMappings();

        await _dbContainer.StartAsync();
    }

    public new async ValueTask DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (dbDescriptor is not null)
            {
                services.Remove(dbDescriptor);
            }

            var auditDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AuditSaveChangesInterceptor));
            if (auditDescriptor is not null)
            {
                services.Remove(auditDescriptor);
            }

            var dispatchDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DispatchDomainEventsInterceptor));
            if (dispatchDescriptor is not null)
            {
                services.Remove(dispatchDescriptor);
            }

            services.AddScoped<AuditSaveChangesInterceptor>();
            services.AddScoped<DispatchDomainEventsInterceptor>();

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());

                var auditInterceptor = sp.GetRequiredService<AuditSaveChangesInterceptor>();
                var dispatchInterceptor = sp.GetRequiredService<DispatchDomainEventsInterceptor>();
                options.AddInterceptors(auditInterceptor, dispatchInterceptor);
            });

            services.RemoveAll<IConfigureOptions<RateLimiterOptions>>();
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ => RateLimitPartition.GetNoLimiter("test"));
            });
        });
    }

    public IConfiguration GetConfig()
    {
        _configuration ??= Services.GetRequiredService<IConfiguration>();
        return _configuration;
    }
}
