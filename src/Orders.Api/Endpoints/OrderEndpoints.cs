using Orders.Api.Repositories;

namespace Orders.Api.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        /**
         * [GET] /orders
         */
        app.MapGet("/orders",
            async (OrderRepository repository) =>
            {
                var listado = await repository.GetAllAsync();
                return TypedResults.Ok(listado);
            });

        /**
         * [POST] /orders
         */
        app.MapPost("/orders",
            async (Models.Order order, OrderRepository repository) =>
            {
                var entidad = await repository.AddAsync(order);

                return TypedResults.Created($"/orders/{entidad.Id}", entidad);
            });

        return app;
    }
}