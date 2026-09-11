using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Config;

public static class ModelValidationConfig
{
    public static IServiceCollection AddModelValidation(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .SelectMany(e => e.Value!.Errors.Select(err => err.ErrorMessage))
                    .ToList();

                return new BadRequestObjectResult(new
                {
                    statusCode = 400,
                    message = errors,
                    error = "Bad Request"
                });
            };
        });

        return services;
    }
}