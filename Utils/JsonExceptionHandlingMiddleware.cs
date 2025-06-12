using System.Text.Json;

namespace Utils
{
    public class JsonExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (JsonException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                string result = JsonSerializer.Serialize(
                    new { Errors = new[] { "Erro na leitura do JSON: " + ex.Message } }
                );

                await context.Response.WriteAsync(result);
            }
        }
    }
}
