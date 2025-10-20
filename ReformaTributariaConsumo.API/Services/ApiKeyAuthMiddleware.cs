namespace ReformaTributaria.API.Services;

public class ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private const string ApiKeyHeaderName = "x-api-key";

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key não encontrada");
            return;
        }

        var apiKey = configuration.GetValue<string>("ApiKey");

        if (apiKey != null && !apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key inválida");
            return;
        }

        await next(context);
    }
}