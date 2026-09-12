using Ecommerce_api.Exceptions;

namespace Ecommerce_api.Middleware;

public static class MiddlewareExtension
{
    public static WebApplication AddMiddleware(this WebApplication app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        return app;
    }
}