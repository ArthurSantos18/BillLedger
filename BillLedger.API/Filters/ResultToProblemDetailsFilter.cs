namespace BillLedger.API.Filters;

public class ResultToProblemDetailsFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is Error error)
        {
            var problemDetails = new ProblemDetails
            {
                Status = error.Code,
                Title = GetTitleForStatus(error.Code),
                Detail = error.Message,
                Instance = context.HttpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = error.Code,
                ContentTypes = { "application/problem+json" }
            };
        }
    }

    public void OnResultExecuted(ResultExecutedContext context) { }

    private static string GetTitleForStatus(int statusCode) => statusCode switch
    {
        400 => "Requisição Inválida",
        404 => "Não Encontrado",
        409 => "Conflito de Estado",
        _ => "Erro na Requisição"
    };
}
