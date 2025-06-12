using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Modules.Guard.Module;

public static class GuardModule
{
    public static IServiceCollection AddGuardModule(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter(
                policyName: "fixed",
                fixedOptions =>
                {
                    fixedOptions.PermitLimit = 5;
                    fixedOptions.Window = TimeSpan.FromSeconds(10);
                    fixedOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    fixedOptions.QueueLimit = 2;
                }
            );

            options.AddPolicy(
                "authenticated",
                partitionContext =>
                {
                    string partitionKey;

                    if (partitionContext.User.Identity?.IsAuthenticated == true)
                    {
                        partitionKey = partitionContext.User.Identity.Name!;
                    }
                    else
                    {
                        partitionKey =
                            partitionContext.Connection.RemoteIpAddress?.ToString()
                            ?? Guid.NewGuid().ToString();
                    }

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: partitionKey,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 20,
                            Window = TimeSpan.FromMinutes(1),
                        }
                    );
                }
            );

            options.AddPolicy(
                "unauthenticatedIp",
                partitionContext =>
                {
                    string partitionKey =
                        partitionContext.Connection.RemoteIpAddress?.ToString()
                        ?? Guid.NewGuid().ToString();

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: partitionKey,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                        }
                    );
                }
            );

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync(
                    "Muitas requisições. Por favor, tente novamente mais tarde.",
                    cancellationToken
                );
            };
        });

        return services;
    }

    public static IApplicationBuilder UseGuardModule(this IApplicationBuilder app)
    {
        app.UseRateLimiter();
        return app;
    }
}
