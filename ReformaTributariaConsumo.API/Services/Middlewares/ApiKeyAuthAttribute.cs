using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ReformaTributaria.API.Services;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("x-api-key", out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key não encontrada");
            return;
        }

        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = configuration.GetValue<string>("ApiKey");

        if (apiKey == null || apiKey.Equals(extractedApiKey)) return;

        context.Result = new UnauthorizedObjectResult("API Key inválida");
    }
}