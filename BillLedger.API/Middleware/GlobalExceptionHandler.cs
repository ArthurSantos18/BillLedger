namespace BillLedger.API.Middleware;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment _environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var correlationId = httpContext.Response.Headers["X-Correlation-Id"].FirstOrDefault() ?? httpContext.TraceIdentifier;

        var (statusCode, title, typeUri) = exception switch
        {
            StateConflictException => (StatusCodes.Status409Conflict, "Conflito de Estado", "https://tools.ietf.org/html/rfc7231#section-6.5.8"),

            DomainException => (StatusCodes.Status400BadRequest, "Regra de Negócio Violada", "https://tools.ietf.org/html/rfc7231#section-6.5.1"),

            _ => (StatusCodes.Status500InternalServerError, "Um erro inesperado ocorreu.", "https://tools.ietf.org/html/rfc7231#section-6.6.1")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(
                exception,
                "Unhandled exception. Path: {Path}, Method: {Method}",
                httpContext.Request.Path,
                httpContext.Request.Method);
        }
        else
        {
            logger.LogWarning(
                "Domain exception: {Message}. Path: {Path}",
                exception.Message,
                httpContext.Request.Path);
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = typeUri,
            Instance = httpContext.Request.Path,
            Detail = (statusCode != StatusCodes.Status500InternalServerError || _environment.IsDevelopment())
                ? exception.Message
                : "Ocorreu um erro interno no servidor. Por favor, contate o suporte."
        };

        problemDetails.Extensions["correlationId"] = correlationId;

        if (statusCode == StatusCodes.Status500InternalServerError && _environment.IsDevelopment())
        {
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            problemDetails.Extensions["exceptionType"] = exception.GetType().Name;
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
