namespace Framework.Infrastructure.Web.Telemetry
{
    internal static class OpenTelemetryInstrumentationExtensions
    {
        internal static IServiceCollection AddOpenTelemetryInstrumentation(this IServiceCollection services, params string[] sourceServices)
        {
            services.AddOpenTelemetry()
                .WithMetrics(metrices =>
                {
                    metrices
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddRuntimeInstrumentation();

                })
                .WithTracing(tracing =>
                {
                    tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddOtlpExporter()
                    .AddConsoleExporter()
                    .AddSource(sourceServices);
                }
                )
                .WithLogging(logging =>
                {
                    logging
                    .AddConsoleExporter();
                }
                );

            return services;
        }
    }
}
