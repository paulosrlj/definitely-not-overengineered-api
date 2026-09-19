using Ecommerce_api.Features.Orders.Create;
using Ecommerce_api.Features.Orders.FindAll;

namespace Ecommerce_api.Features.Orders;

public static class OrderServiceExtension
{
    public static IServiceCollection AddOrderFeature(
        this IServiceCollection services)
    {
        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<FindAllOrdersHandler>();

        return services;
    }
}