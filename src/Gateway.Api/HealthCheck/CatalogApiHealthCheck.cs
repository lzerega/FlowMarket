using Gateway.Api.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Gateway.Api.HealthCheck
{
    public class CatalogApiHealthCheck : IHealthCheck
    {
        private CatalogService _catalogService;

        public CatalogApiHealthCheck(CatalogService catalogService)
        {
            this._catalogService = catalogService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var hablitado = await this._catalogService.IsAvailableAsync(cancellationToken);

            return hablitado ? HealthCheckResult.Healthy("Catalogo Api Habilitado")
                : HealthCheckResult.Unhealthy("Catalogo Api Inhabilitado");
        }
    }
}
