using Gateway.Api.Configurations;
using Gateway.Api.Endpoints;
using Gateway.Api.Extensions;
using Gateway.Api.HealthCheck;
using Gateway.Api.HealthChecks;
using Gateway.Api.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Transactions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ServicesOptions>(
    builder.Configuration.GetSection(ServicesOptions.SectionName));

// Add services to the container.
builder.Services.AddCustomOpenTelemetry(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
        .AddCheck<CatalogApiHealthCheck>("catalog-api")
        .AddCheck<OrderApiHealthCheck>("orders-api");


builder.Services.AddHttpClient<CatalogService>((serviceProvider, client) =>
{
    //var cat = builder.Configuration["Services:Catalog"];
    var options = serviceProvider
        .GetRequiredService<IOptions<ServicesOptions>>()
        .Value;

    client.BaseAddress = new Uri(options.Catalog);
});
builder.Services.AddHttpClient<OrderService>((serviceProvider, client) =>
{
    //var cat = builder.Configuration["Services:Orders"];
    var options = serviceProvider
        .GetRequiredService<IOptions<ServicesOptions>>()
        .Value;

    client.BaseAddress = new Uri(options.Orders);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

/**
 * Endpoint para gateway
 * 
 */
app.MapGatewayEndpoints();

/**
 * Endpoint para health check
 * 
 */
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = HealthCheckResponseWriter.WriteResponse
});

app.Run();