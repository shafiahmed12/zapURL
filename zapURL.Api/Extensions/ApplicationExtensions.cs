using zapURL.Api.Services.UrlService;
using zapURL.Api.Utilities;

namespace zapURL.Api.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var redisConnectionString = configuration.GetConnectionString("RedisCache");

        services.AddScoped<IUrlService, UrlService>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        if (connectionString != null && redisConnectionString != null)
            services.AddHealthChecks()
                .AddNpgSql(connectionString)
                .AddRedis(redisConnectionString);

        return services;
    }
}