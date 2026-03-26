
namespace Framework.Infrastructure.Web.ExceptionHandler
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            var statusCode = exception switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            httpContext.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Title = GetTitle(statusCode),
                Detail = GetDetail(exception, httpContext.RequestServices.GetRequiredService<IHostEnvironment>()),
                Instance = httpContext.Request.Path,
                Type = $"https://httpstatuses.com/{statusCode}",
                Extensions =
                 {
                     { "traceId", httpContext.TraceIdentifier } // Include trace ID for correlation
                 }

            };
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            // Return true to stop the exception from propagating further
            return true;

        }

        private static string GetTitle(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            500 => "Internal Server Error",
            _ => "An error occurred"
        };

        private static string GetDetail(Exception exception, IHostEnvironment env)
        {
            return env.IsDevelopment()
                ? exception.Message
                : "An unexpected error occurred. Please try again later.";
        }

        public async ValueTask<bool> TryHandleAsync2(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
            var error = exceptionHandlerFeature?.Error;
            if (exception != null)
            {
                _logger.LogError(exception, "An unhandled exception occurred.");
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    Message = "An unexpected error occurred. Please try again later."
                };
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
            return true;
        }

    }
}
