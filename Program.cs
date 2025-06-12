using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Middlewares;
using Modules.Auth.Module;
using Modules.Category.Module;
using Modules.Comments.Module;
using Modules.Guard.Module;
// Certifique-se de que este using está presente
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

// Adição dos módulos de serviço
builder.Services.AddUserModule();
builder.Services.AddNewsModule();
builder.Services.AddCommentModule();
builder.Services.AddCategoryModule();
builder.Services.AddAuthModule();
builder.Services.AddGuardModule(); // Configura as políticas de Rate Limiting nos serviços

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
                Array.Empty<string>()
            },
        }
    );
});

WebApplication app = builder.Build();

app.UseHttpsRedirection();

// Adiciona o middleware de roteamento
app.UseRouting(); // Boa prática adicionar explicitamente antes de UseAuthentication/UseAuthorization

app.UseAuthentication();
app.UseAuthorization();

// IMPORTANTE: Adicione o GuardModule (com Rate Limiting) AQUI,
// após a autenticação e autorização para que as políticas que dependem do usuário funcionem.
app.UseGuardModule(); // Adiciona o middleware de Rate Limiting

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
