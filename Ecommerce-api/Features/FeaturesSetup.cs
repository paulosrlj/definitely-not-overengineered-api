using Ecommerce_api.Features.Auth;
using Ecommerce_api.Features.Categories;
using Ecommerce_api.Features.Products;
using Ecommerce_api.Features.Users;

namespace Ecommerce_api.Features;

public static class FeaturesSetup
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        services.AddAuthFeature();
        services.AddUserFeature();
        services.AddCategoryFeature();
        services.AddProductFeature();

        return services;
    }
}