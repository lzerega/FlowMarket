using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Catalog.Api.Services;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Catalog.Api.Configurations;
using Npgsql;
using OpenTelemetry.Metrics;

namespace Catalog.Api.Extensions
{
    public static class OpenTelemetryExtensions
    {
        public static IServiceCollection AddCustomOpenTelemetry(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Bind directo de la sección a la clase de opciones
            var otelOptions = configuration
                .GetSection(OpenTelemetryOptions.SectionName)
                .Get<OpenTelemetryOptions>() ?? new OpenTelemetryOptions();

            // Fallback por si la variable de entorno de Docker sobreescribe el valor
            var otlEndpoint = string.IsNullOrWhiteSpace(otelOptions.Endpoint)
                ? configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? otelOptions.Endpoint
                : "http://otel-collector:4317";

            var serviceName = string.IsNullOrWhiteSpace(otelOptions.ServiceName)
                ? "unknown-service"
                : otelOptions.ServiceName;

            // Registrar OpenTelemetry
            services.AddOpenTelemetry()
                .ConfigureResource(resource => 
                {
                    resource.AddService(serviceName);
                })
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddNpgsql()
                        .AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri(otlEndpoint);
                            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        });
                })
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddMeter("Catalog.Api")
                        .AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri(otlEndpoint);
                            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        });
                });

            // Opcional: Registrar IOptions<OpenTelemetryOptions> en el contenedor de DI por si se necesita inyectar en otro servicio
            services.Configure<OpenTelemetryOptions>(
                configuration.GetSection(OpenTelemetryOptions.SectionName));

            return services;
        }
        
    }
}
