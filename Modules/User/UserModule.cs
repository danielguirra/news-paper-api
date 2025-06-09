using Modules.User.Controller;
using Modules.User.Service;

namespace Modules.User.Module;

public static class UserModule
{
    public static IServiceCollection AddUserModule(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddControllers().AddApplicationPart(typeof(UserController).Assembly);
        return services;
    }
}
