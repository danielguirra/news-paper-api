using Data;
using Microsoft.EntityFrameworkCore;
using Middlewares;
using Modules.Auth;
using Modules.Category;
using Modules.Comments;
using Modules.News;
using Modules.User;
using Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder
    .Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAndHandleErrorsFilter>();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddUserModule();
builder.Services.AddNewsModule();
builder.Services.AddCommentModule();
builder.Services.AddCategoryModule();
builder.Services.AddAuthModule();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<JsonExceptionHandlingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
