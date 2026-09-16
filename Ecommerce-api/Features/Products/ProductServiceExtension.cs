using Ecommerce_api.Features.Products.Create;
using Ecommerce_api.Features.Products.Delete;
using Ecommerce_api.Features.Products.FindAll;
using Ecommerce_api.Features.Products.FindOne;
using Ecommerce_api.Features.Products.Search;
using Ecommerce_api.Features.Products.Update;

namespace Ecommerce_api.Features.Products;

public static class ProductServiceExtension
{
    public static IServiceCollection AddProductFeature(
        this IServiceCollection services)
    {
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<FindAllProductsHandler>();
        services.AddScoped<FindOneProductHandler>();
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<DeleteProductHandler>();
        services.AddScoped<SearchProductsHandler>();
        
        return services;
    }
}