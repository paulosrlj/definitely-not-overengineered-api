using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(
                ex,
                "Resource not found. {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status404NotFound;

            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (ConflictException ex)
        {
            _logger.LogWarning(
                ex,
                "Conflict occurred. {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status409Conflict;

            await context.Response.WriteAsJsonAsync(
                new { message = ex.Message });
        }
        catch (BadCredentialsException ex)
        {
            _logger.LogWarning(
                ex,
                "Authentication failed. {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad credentials",
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.2"
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception. {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred."
                });
        }
    }
}