using Serilog;

namespace Ecommerce_api.Config;

public static class SerilogConfig
{
    public static IHostBuilder SetupSerilog(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
            .WriteTo.File(
                new Serilog.Formatting.Json.JsonFormatter(),
                "logs/logs.json",
                rollingInterval: RollingInterval.Day
            ));

        return hostBuilder;
    }
}