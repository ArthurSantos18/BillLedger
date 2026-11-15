namespace BillLedger.API.Extensions;

public static class RateLimitingExtension
{
    public static IServiceCollection AddBillLedgerRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.OnRejected = async (context, cancellationToken) =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("BillLedger.RateLimiting");

                var path = context.HttpContext.Request.Path.ToString();
                var method = context.HttpContext.Request.Method;
                var retryAfter = GetRetryAfter(context);

                logger.LogWarning(
                    "Rate limit exceeded on {Method} {Path}. RetryAfter: {RetryAfter}s",
                    method,
                    path,
                    retryAfter);

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status429TooManyRequests,
                    Title = "Muitas requisições.",
                    Detail = "Você excedeu o limite de requisições. Tente novamente mais tarde.",
                    Instance = context.HttpContext.Request.Path
                };

                if (retryAfter != null)
                {
                    context.HttpContext.Response.Headers.RetryAfter = retryAfter.ToString();
                }

                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/problem+json";

                await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            };

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetSlidingWindowLimiter(
                    ip, _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 200,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 6,
                        QueueLimit = 0
                    });
            });
        });

        return services;
    }

    public static WebApplication UseBillLedgerRateLimiting(this WebApplication app)
    {
        app.UseRateLimiter();
        return app;
    }

    private static int? GetRetryAfter(OnRejectedContext context)
    {
        if (!context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            return null;
        }

        return Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
    }
}
