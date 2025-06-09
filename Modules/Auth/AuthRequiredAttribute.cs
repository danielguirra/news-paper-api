using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Modules.Auth.Service;

namespace Modules.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthRequiredAttribute(params string[] roles) : Attribute, IAsyncAuthorizationFilter
{
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

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, auth.Id.ToString()),
            new Claim(ClaimTypes.Name, auth.Name),
            new Claim(ClaimTypes.Role, auth.Role),
        };
        var userIdentity = new ClaimsIdentity(claims, "AuthModel"); //
        context.HttpContext.User = new ClaimsPrincipal(userIdentity);

        if (auth.Role == "boss" || auth.Role == "admin")
            return;

        if (roles.Length > 0 && !roles.Contains(auth.Role))
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
