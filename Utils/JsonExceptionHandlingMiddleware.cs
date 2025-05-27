using System.Text.Json;


namespace Utils
{
  public class JsonExceptionHandlingMiddleware
  {
    public JsonExceptionHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
    }
    private readonly RequestDelegate _next;
    

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

        var result = JsonSerializer.Serialize(new
        {
          Errors = new[] { "Erro na leitura do JSON: " + ex.Message }
        });

        await context.Response.WriteAsync(result);
      }
    }
  }

}
