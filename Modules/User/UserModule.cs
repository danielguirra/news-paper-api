using Controllers;

using Services;

namespace Modules.User;

public static class UserModule
{
  public static IServiceCollection AddUserModule(this IServiceCollection services)
  {
    services.AddScoped<UserService>();
    services.AddControllers().AddApplicationPart(typeof(UserController).Assembly);
    return services;
  }
}
