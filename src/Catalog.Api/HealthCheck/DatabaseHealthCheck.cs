using Catalog.Api.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Catalog.Api.HealthCheck
{
    public class DatabaseHealthCheck(AppDbContext dbContext) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);

                return canConnect
                    ? HealthCheckResult.Healthy("Conexión a PostgreSQL correcta.")
                    : HealthCheckResult.Unhealthy("No se pudo conectar a la base de datos.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Error al verificar la base de datos.", ex);
            }
        }
    }
}
