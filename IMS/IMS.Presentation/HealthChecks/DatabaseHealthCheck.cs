using IMS.DAL;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IMS.Presentation.HealthChecks;

public class DatabaseHealthCheck(IMSDbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);

            if (canConnect) return HealthCheckResult.Healthy("Database connection is healthy");

            return HealthCheckResult.Unhealthy("Database connection failed");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("Database health check failed", exception);
        }
    }
}
