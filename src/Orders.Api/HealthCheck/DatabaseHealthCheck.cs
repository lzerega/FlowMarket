using Orders.Api.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orders.Api.HealthCheck
{
    public class DatabaseHealthCheck(OrdersDbContext dbContext) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);

                return canConnect
                    ? HealthCheckResult.Healthy("Conexión a PostgreSQL order-db correcta.")
                    : HealthCheckResult.Unhealthy("No se pudo conectar a la base de datos order-db.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Error al verificar la base de datos order-db.", ex);
            }
        }
    }
}
