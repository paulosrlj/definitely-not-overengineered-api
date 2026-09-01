using Ecommerce_api.Features.Users.Create;
using Ecommerce_api.Features.Users.Delete;
using Ecommerce_api.Features.Users.FindAll;
using Ecommerce_api.Features.Users.FindOne;
using Ecommerce_api.Features.Users.Update;

namespace Ecommerce_api.Features.Users;

public static class UserServiceExtension
{
    public static IServiceCollection AddUserFeature(
        this IServiceCollection services)
    {
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<UpdateUserHandler>();
        services.AddScoped<FindAllUsersHandler>();
        services.AddScoped<FindOneUserHandler>();
        services.AddScoped<DeleteUserHandler>();

        return services;
    }
}