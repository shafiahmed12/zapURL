using Microsoft.EntityFrameworkCore;
using zapURL.Api.Data;
using zapURL.Api.Data.Repositories.ShortenUrlRepository;

namespace zapURL.Api.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ZapUrlDbContext>(options => { options.UseNpgsql(connectionString); });
        services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
        services.AddRedis(configuration);
        return services;
    }

    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("RedisCache");
        services.AddStackExchangeRedisCache(options => { options.Configuration = redisConnectionString; });
    }
}