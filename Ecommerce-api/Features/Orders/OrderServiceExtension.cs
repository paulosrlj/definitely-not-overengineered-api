using Ecommerce_api.Features.Orders.Create;

namespace Ecommerce_api.Features.Orders;

public static class OrderServiceExtension
{
    public static IServiceCollection AddOrderFeature(
        this IServiceCollection services)
    {
        services.AddScoped<CreateOrderHandler>();

        return services;
    }
}