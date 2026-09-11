using Ecommerce_api.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Config;

public static class RedisConfig
{
    public static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "SampleInstance:";
        });

        return services;
    }
}