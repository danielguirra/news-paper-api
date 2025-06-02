namespace Modules.Category;

public static class CategoryModule
{
    public static IServiceCollection AddCategoryModule(this IServiceCollection services)
    {
        services.AddScoped<CategoryService>();
        services.AddControllers().AddApplicationPart(typeof(CategoryController).Assembly);
        return services;
    }
}
