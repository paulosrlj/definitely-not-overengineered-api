namespace Ecommerce_api.Middleware;

public class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-ID";

    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(
        RequestDelegate next,
        ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = GetCorrelationId(context);

        context.Response.Headers[HeaderName] = correlationId;

        using var scope = _logger.BeginScope(
            "CorrelationId: {CorrelationId}",
            correlationId);

        _logger.LogInformation(
            "HTTP request started: {Method} {Path}",
            context.Request.Method,
            context.Request.Path);

        await _next(context);

        _logger.LogInformation(
            "HTTP request completed: {StatusCode}",
            context.Response.StatusCode);
    }

    private static string GetCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(
                HeaderName,
                out var existingId) &&
            Guid.TryParse(existingId, out var parsedId))
        {
            return parsedId.ToString();
        }

        return Guid.NewGuid().ToString();
    }
}