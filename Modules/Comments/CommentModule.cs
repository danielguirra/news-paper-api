namespace Modules.Comments
{
    public static class CommentModule
    {
        public static IServiceCollection AddCommentModule(this IServiceCollection services)
        {
            services.AddScoped<CommentService>();
            services.AddControllers().AddApplicationPart(typeof(CommentController).Assembly);
            return services;
        }
    }
}
