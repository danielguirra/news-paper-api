namespace Modules.Auth;

public static class AuthModule
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();

        return services;
    }
}
