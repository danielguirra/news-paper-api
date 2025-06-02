using Exceptions;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger
)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var code = 500;
            var message = "Erro interno do servidor";

            if (ex is IHasHttpCode httpCode)
            {
                code = httpCode.Code;
                message = ex.Message;
            }

            _logger.LogError(ex, "Exceção capturada no middleware.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;

            var response = new { error = message };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
