using Catalog.Api.Configurations;
using Catalog.Api.Data;
using Catalog.Api.Endpoints;
using Catalog.Api.Models;
using Catalog.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.Extensions.DependencyInjection;
using Catalog.Api.Extensions;
using Catalog.Api.HealthCheck;

string ReadSecret(string name)
{
    var paths = new[]
    {
        $"/run/secrets/{name}",
        $"/etc/secrets/{name}"
    };

    foreach (var path in paths)
    {
        if (File.Exists(path))
            return File.ReadAllText(path).Trim();
    }
    return "";
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomOpenTelemetry(builder.Configuration);

// Configurar primero las conecciones a la base
builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection(DatabaseOptions.SectionName)
);

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
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
builder.Services.AddScoped<CatalogRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
}

app.MapProductEndpoint();

app.MapHealthChecks("/health");

app.Run();
