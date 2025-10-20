using Dapper;

using Microsoft.Extensions.Diagnostics.HealthChecks;

using System.Data;

namespace ReformaTributaria.API.Utils.DB.HealthChecks;

public class DatabaseHealthCheck(IDbConnection conn) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        try
        {
            await conn.ExecuteAsync("select 1");
            return HealthCheckResult.Healthy(data: new Dictionary<string, object> { { "Database", conn.Database.ToLower() } });
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(description: conn.Database.ToLower(), exception: ex);
        }
    }
}
