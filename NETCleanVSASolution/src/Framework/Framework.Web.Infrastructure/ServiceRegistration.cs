namespace Framework.Infrastructure.Web
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddFrameworkWebInfrastructure(this IServiceCollection services, IConfiguration configuration, params string[] sourceServices)
        {
            //services.AddOpenTelemetryInstrumentation(sourceServices);

            services.AddHttpLoggingRegistration();
            services.AddProblemDetailsRegistration();
            services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }

        private static IServiceCollection AddHttpLoggingRegistration(this IServiceCollection services)
        {
            //Add built-in HttpLogging middleware to log HTTP request and response data. You can customize the logging options as needed.
            services.AddHttpLogging(options =>
            {
                options.LoggingFields =
                HttpLoggingFields.RequestMethod |
                HttpLoggingFields.RequestPath |
                HttpLoggingFields.RequestQuery |
                HttpLoggingFields.RequestHeaders |
                HttpLoggingFields.Response |
                HttpLoggingFields.ResponseStatusCode |
                HttpLoggingFields.Duration;
            });
            return services;
        }
        private static IServiceCollection AddProblemDetailsRegistration(this IServiceCollection services)
        {
            services.AddProblemDetails((options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance = context.HttpContext.Request.Path;
                    context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                    // Add additional metadata
                    if (context.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment())
                    {
                        context.ProblemDetails.Extensions["environment"] = "Development";
                    }
                };
            }));
            return services;
        }
    }
}