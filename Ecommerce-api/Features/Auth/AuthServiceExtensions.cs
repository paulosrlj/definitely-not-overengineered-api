using Ecommerce_api.Features.Auth.Signin;
using Ecommerce_api.Features.Auth.Signup;

namespace Ecommerce_api.Features.Auth;

public static class AuthServiceExtension
{
    public static IServiceCollection AddAuthFeature(
        this IServiceCollection services)
    {
        services.AddScoped<SigninHandler>();
        services.AddScoped<SignupHandler>();

        return services;
    }
}