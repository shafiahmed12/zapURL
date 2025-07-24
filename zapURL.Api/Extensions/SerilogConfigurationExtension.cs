using Serilog;

namespace zapURL.Api.Extensions;

public static class SerilogConfigurationExtension
{
    public static void AddSerilog(this IHostBuilder host)
    {
        host.UseSerilog((context, loggerConfig) => { loggerConfig.WriteTo.Console(); });
    }
}