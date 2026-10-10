using Gateway.Api.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Gateway.Api.HealthCheck
{
    public class OrderApiHealthCheck : IHealthCheck
    {
        private OrderService _orderService;

        public OrderApiHealthCheck(OrderService orderService)
        {
            this._orderService = orderService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var hablitado = await this._orderService.IsAvailableAsync(cancellationToken);

            return hablitado ? HealthCheckResult.Healthy("Orders Api Habilitado")
                : HealthCheckResult.Unhealthy("Orders Api Inhabilitado");
        }
    }
}
