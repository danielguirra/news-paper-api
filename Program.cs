using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Middlewares;
using Modules.Auth.Module;
using Modules.Category.Module;
using Modules.Comments.Module;
using Modules.News.Module;
using Modules.User.Module;
using Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "API de Notícias",
            Description = "Uma API para gerenciar notícias, categorias, usuários e comentários.",
            Contact = new OpenApiContact
            {
                Name = "Daniel Guirra",
                Email = "daniel.guirra777@gmail.com",
            },
            License = new OpenApiLicense
            {
                Name = "Licença MIT",
                Url = new Uri("https://opensource.org/license/mit"),
            },
        }
    );

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Insira o token JWT no formato: `Bearer {token}`",
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        }
    );
});

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Notícias v1");
        options.EnablePersistAuthorization();
        options.DisplayOperationId();
    });
}

app.MapControllers();
app.UseMiddleware<JsonExceptionHandlingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
