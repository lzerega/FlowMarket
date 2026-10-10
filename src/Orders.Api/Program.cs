using Orders.Api.Configurations;
using Microsoft.Extensions.Options;
using Orders.Api.Endpoints;
using Orders.Api.Models;
using Orders.Api.Data;
using Orders.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Orders.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configurar primero las conecciones a la base
builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection(DatabaseOptions.SectionName)
);

// Add services to the container.
builder.Services.AddCustomOpenTelemetry(builder.Configuration);

builder.Services.AddDbContext<OrdersDbContext>((serviceProvider, options) =>
{
    var databaseOptions =
    serviceProvider
        .GetRequiredService<IOptions<DatabaseOptions>>()
        .Value;
    options.UseNpgsql(databaseOptions.ConnectionString);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<OrderRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapOrderEndpoints();
app.MapHealthChecks("/health");

app.Run();
