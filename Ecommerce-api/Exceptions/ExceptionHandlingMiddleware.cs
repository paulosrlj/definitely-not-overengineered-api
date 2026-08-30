using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (ConflictException ex)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (BadCredentialsException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(new ProblemDetails(){
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad credentials",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.2"
            });
        }

    }
}