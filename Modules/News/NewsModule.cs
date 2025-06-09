using Modules.News.Controller;
using Modules.News.Service;

namespace Modules.News.Module;

public static class NewsModule
{
    public static IServiceCollection AddNewsModule(this IServiceCollection services)
    {
        services.AddScoped<NewsService>();
        services.AddControllers().AddApplicationPart(typeof(NewsController).Assembly);
        return services;
    }
}
