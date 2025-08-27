using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using zapURL.Api.Configurations;
using zapURL.Api.Infrastructure.AuthClient;
using zapURL.Api.Services.AuthenticationService;
using zapURL.Api.Services.JwksService;
using zapURL.Api.Services.UrlService;
using zapURL.Api.Utilities;

namespace zapURL.Api.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var redisConnectionString = configuration.GetConnectionString("RedisCache");
        var stackAuthSettings = configuration.GetSection(StackAuthSettings.SectionName)!;
        services.AddHttpClient();
        services.Configure<StackAuthSettings>(stackAuthSettings);

        services.AddSingleton<IJwksService, JwksService>();
        services.AddScoped<IUrlService, UrlService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        if (connectionString != null && redisConnectionString != null)
            services.AddHealthChecks()
                .AddNpgSql(connectionString)
                .AddRedis(redisConnectionString);

        ConfigureTypedHttpClient(services, stackAuthSettings.Get<StackAuthSettings>()!);
        ConfigureAuthentication(services, stackAuthSettings.Get<StackAuthSettings>()!);
        return services;
    }

    private static void ConfigureTypedHttpClient(this IServiceCollection services, StackAuthSettings stackAuthSettings)
    {
        services.AddHttpClient<IStackAuthClient, StackAuthClient>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(StackAuthSettings.BaseAddress);
            httpClient.DefaultRequestHeaders.Add(StackAuthSettings.AccessTypeHeader, stackAuthSettings.AccessType);
            httpClient.DefaultRequestHeaders.Add(StackAuthSettings.ProjectIdHeader, stackAuthSettings.ProjectId);
            httpClient.DefaultRequestHeaders.Add(StackAuthSettings.SecretServerKeyHeader,
                stackAuthSettings.SecretServerKey);
        });
    }

    private static void ConfigureAuthentication(this IServiceCollection services, StackAuthSettings stackAuthSettings)
    {
        // Register the handler
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IJwksService>((jwtBearerOptions, jwksService) =>
            {
                jwtBearerOptions.Authority = stackAuthSettings.Authority;
                var jwks = jwksService.GetJwksAsync().GetAwaiter().GetResult();

                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = StackAuthSettings.ValidIssuer,
                    ValidAudience = stackAuthSettings.ProjectId,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeys = jwks.Keys
                };
            });
    }
}