using Modules.Auth.Model;
using Modules.User.Service;

namespace Modules.Auth.Service;

public class AuthService(IHttpContextAccessor httpContextAccessor, UserService userService)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserService _userService = userService;

    public async Task<AuthModel?> GetAuthUser()
    {
        string? authHeader =
            _httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
            return null;

        string? token = authHeader["Bearer ".Length..].Trim();
        return await _userService.Me(token);
    }
}
