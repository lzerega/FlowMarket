using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Gateway.Api.Services;

namespace Gateway.Api.Endpoints
{
    public static class OpenTelemetryExtensions
    {
        public static IEndpointRouteBuilder MapGatewayEndpoints(this IEndpointRouteBuilder app)
        {
            /*
             * GET /orders
             * 
             */
            app.MapGet("/", async (CatalogService catalogService) =>
            {
                var products = await catalogService.GetProductsAsync();

                return TypedResults.Ok(new
                {
                    Service = "Gateway.Api",
                    Status = "Running",
                    Timestamp = DateTime.UtcNow,
                    Products = products,
                    ProductCount = products.Count

                });
            })
            .WithName("GatewayHealth")
            .WithOpenApi();

            /*
             * POST
             * 
             */

            return app;
        }

    }
}
