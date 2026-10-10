using Catalog.Api.Models;
using Catalog.Api.Repositories;
using System.Security.Cryptography.X509Certificates;

namespace Catalog.Api.Endpoints
{
    public static class CatalogEndpoint
    {
        public static IEndpointRouteBuilder MapProductEndpoint(this IEndpointRouteBuilder app)
        {
            /**
         * [GET] /orders
         */
            app.MapGet("/products",
                async (CatalogRepository repository) =>
                {
                    var list = await repository.GetAllAsync();
                    return TypedResults.Ok(list);
                })
            .WithName("Get");

            /**
             * [POST] /orders
             */
            app.MapPost("/products",
                async (Models.Product product, CatalogRepository repository) =>
                {
                    product = await repository.AddAsync(product);
                    return TypedResults.Created($"/products/{product.Id}", product);
                })
                .WithName("Post");

            return app;
        }
    }
}
