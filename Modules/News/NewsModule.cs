namespace Modules.News;

public static class NewsModule
{
    public static IServiceCollection AddNewsModule(this IServiceCollection services)
    {
        services.AddScoped<NewsService>();
        services.AddControllers().AddApplicationPart(typeof(NewsController).Assembly);
        return services;
    }
}
