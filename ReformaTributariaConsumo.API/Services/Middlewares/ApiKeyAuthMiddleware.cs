using Microsoft.AspNetCore.Authorization;

namespace ReformaTributaria.API.Services.Middlewares;

public class ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private const string ApiKeyHeaderName = "x-api-key";

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.StatusCode = 200;
        
        if (context.Request.Path.Value != null && context.Request.Path.Value.Contains("healthz"))
        {
            await next(context);
            return;
        }

        var endpoint = context.GetEndpoint();
        if (endpoint is not null)
        {
            var isAllowAnnonimous = endpoint.Metadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));

            if (isAllowAnnonimous)
            {
                await next(context);
                return;
            }
        }

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