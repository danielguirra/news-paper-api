using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Modules.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthRequiredAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _roles;

    public AuthRequiredAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var authService = context.HttpContext.RequestServices.GetService<AuthService>();
        var auth = authService == null ? null : await authService.GetAuthUser();

        if (auth == null)
        {
            context.Result = new UnauthorizedObjectResult(
                new { message = "Token não fornecido e/ou inválido." }
            );
            return;
        }

        if (_roles.Length > 0 && !_roles.Contains(auth.Role))
        {
            context.Result = new ObjectResult(
                new { message = "Você não tem permissão para acessar este recurso." }
            )
            {
                StatusCode = StatusCodes.Status403Forbidden,
            };
        }
    }
}
