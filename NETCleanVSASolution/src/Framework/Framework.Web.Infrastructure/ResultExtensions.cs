namespace Framework.Infrastructure.Web
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResultSimple<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result.Value);

            return result.Error.Code switch
            {
                var code when code.Contains("NotFound") => new NotFoundObjectResult(new { error = result.Error.Message }),
                var code when code.Contains("Unauthorized") => new UnauthorizedObjectResult(new { error = result.Error.Message }),
                var code when code.Contains("Validation") => new BadRequestObjectResult(new { error = result.Error.Message }),
                var code when code.Contains("Conflict") => new ConflictObjectResult(new { error = result.Error.Message }),
                _ => new BadRequestObjectResult(new { error = result.Error.Message })
            };
        }

        public static IActionResult ToActionResult<T>(this Result<T> result, HttpContext? httpContext = null)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result.Value);
            else
                return result.ToProblemDetails(httpContext);
        }

        public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult> onSuccess, HttpContext? httpContext = null)
        {
            if (result.IsSuccess)
                return onSuccess(result.Value);

            return result.ToProblemDetails(httpContext);
        }

        public static IActionResult ToActionResult(this Result result, Func<IActionResult> onSuccess, HttpContext? httpContext = null)
        {
            if (result.IsSuccess)
                return onSuccess();

            return result.ToProblemDetails(httpContext);
        }


        public static IActionResult ToProblemDetails(this Result result, HttpContext? httpContext = null)
        {
            var statusCode = result.Error.Code switch
            {
                var code when code.Contains("NotFound") => StatusCodes.Status404NotFound,
                var code when code.Contains("Unauthorized") => StatusCodes.Status401Unauthorized,
                var code when code.Contains("Forbidden") => StatusCodes.Status403Forbidden,
                var code when code.Contains("Validation") => StatusCodes.Status400BadRequest,
                var code when code.Contains("Conflict") => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest
            };

            var problemDetails = new ProblemDetails
            {
                Type = $"https://api.yourapp.com/errors/{result.Error.Code}",
                Title = GetTitle(result.Error.Code),
                Status = statusCode,
                Detail = result.Error.Message,
                Instance = httpContext?.Request.Path.Value ?? string.Empty
            };

            // Add traceId if available
            if (httpContext != null)
            {
                problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            }

            return new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
        }

        private static string GetTitle(string errorCode)
        {
            return errorCode switch
            {
                var code when code.Contains("NotFound") => "Resource Not Found",
                var code when code.Contains("Unauthorized") => "Unauthorized",
                var code when code.Contains("Forbidden") => "Forbidden",
                var code when code.Contains("Validation") => "Validation Error",
                var code when code.Contains("Conflict") => "Conflict",
                _ => "Bad Request"
            };
        }
    }
}
