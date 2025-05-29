using Models.AuthModel;

namespace Services
{
  public class AuthService(IHttpContextAccessor httpContextAccessor, UserService userService)
  {
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserService _userService = userService;

    public async Task<AuthModel?> GetAuthUser()
    {
      var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault();
      if (authHeader == null || !authHeader.StartsWith("Bearer ")) return null;

      var token = authHeader["Bearer ".Length..].Trim();
      return await _userService.Me(token);
    }
  }
}
