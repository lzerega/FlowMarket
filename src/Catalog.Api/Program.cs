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
using System.Diagnostics;

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

// Leemos los valores usando los nombres de los archivos secretos
var dbUser = ReadSecret("postgres_catalog_db_username");
//esta linea no va es solo por el momento si en caso no hay valor
if (string.IsNullOrEmpty(dbUser)) dbUser = "cataloguser";
var dbPass = ReadSecret("postgres_catalog_db_password");
//esta linea no va es solo por el momento si en caso no hay valor
if (string.IsNullOrEmpty(dbPass)) dbPass = "catalogpassword";

var builder = WebApplication.CreateBuilder(args);

// Configurar primero las conecciones a la base
builder.Services.Configure<DatabaseOptions>((options) =>
{
    builder.Configuration.GetSection(DatabaseOptions.SectionName).Bind(options);

    // Si en el appsettings tengo placeholder {dbUser} / {dbPass}, los reemplazamos
    if (options.Username.Contains("{dbUser}"))
        options.Username = options.Username.Replace("{dbUser}", dbUser);

    if (options.Password.Contains("{dbPass}"))
        options.Password = options.Password.Replace("{dbPass}", dbPass);
});

// Add services to the container.
builder.Services.AddCustomOpenTelemetry(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var dbOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
    var templateConnString = dbOptions.ConnectionString;
    
    Console.WriteLine("***********************");
    Console.WriteLine("Conectando");
    Console.WriteLine(templateConnString);
    options.UseNpgsql(templateConnString);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<CatalogRepository>();

var app = builder.Build();

// Ejecutamos las migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    Console.WriteLine("¡Migraciones aplicadas con éxito usando el patrón Options!");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
}

app.MapProductEndpoint();

app.MapHealthChecks("/health");

app.Run();
