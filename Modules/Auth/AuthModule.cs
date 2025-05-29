using Services;

namespace Modules.News;

public static class AuthModel
{
  public static IServiceCollection AddAuthModule(this IServiceCollection services)
  {
    services.AddScoped<AuthService>();

    return services;
  }
}
