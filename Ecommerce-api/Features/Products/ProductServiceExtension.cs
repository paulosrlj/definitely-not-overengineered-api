using Ecommerce_api.Features.Categories.Create;
using Ecommerce_api.Features.Categories.Delete;
using Ecommerce_api.Features.Categories.FindAll;
using Ecommerce_api.Features.Categories.FindOne;
using Ecommerce_api.Features.Categories.Update;

namespace Ecommerce_api.Features.Products;

public static class ProductServiceExtension
{
    public static IServiceCollection AddproductFeature(
        this IServiceCollection services)
    {
        services.AddScoped<CreateCategoryHandler>();
        services.AddScoped<FindAllCategoriesHandler>();
        services.AddScoped<FindOneCategoryHandler>();
        services.AddScoped<UpdateCategoryHandler>();
        services.AddScoped<DeleteCategoryHandler>();
        
        return services;
    }
}