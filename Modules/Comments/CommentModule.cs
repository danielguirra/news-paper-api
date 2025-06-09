using Modules.Comments.Controller;
using Modules.Comments.Service;

namespace Modules.Comments.Module
{
    public static class CommentsModule
    {
        public static IServiceCollection AddCommentModule(this IServiceCollection services)
        {
            services.AddScoped<CommentsService>();
            services.AddControllers().AddApplicationPart(typeof(CommentsController).Assembly);
            return services;
        }
    }
}
