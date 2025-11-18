using CineMate.Configuration;

namespace CineMate.MiddleEarth;

public class ApiKeyMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private const string ApiKeyHeaderName = "x-api-key";

    public async Task InvokeAsync(HttpContext context, ApiSettings settings)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey) ||
            extractedApiKey != settings.ApiKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
        await _next(context);
    }
}
