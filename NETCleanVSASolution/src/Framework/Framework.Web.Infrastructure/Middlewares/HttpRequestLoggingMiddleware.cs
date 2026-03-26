namespace Framework.Infrastructure.Web.Middlewares
{
    public class HttpRequestLoggingMiddleware(RequestDelegate _next, ILogger<HttpRequestLoggingMiddleware> _logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Enable buffering so we can read the body without consuming it
            context.Request.EnableBuffering();

            string bodyText = string.Empty;

            if (context.Request.ContentLength is > 0 &&
                context.Request.Body.CanRead)
            {
                context.Request.Body.Position = 0;

                using (var reader = new StreamReader(
                           context.Request.Body,
                           Encoding.UTF8,
                           detectEncodingFromByteOrderMarks: false,
                           bufferSize: 1024,
                           leaveOpen: true))
                {
                    bodyText = await reader.ReadToEndAsync().ConfigureAwait(false);
                    context.Request.Body.Position = 0;
                }
            }

            var logObject = new
            {
                Scheme = context.Request.Scheme,
                Host = context.Request.Host.ToString(),
                Path = context.Request.Path.ToString(),
                QueryString = context.Request.QueryString.HasValue
                    ? context.Request.QueryString.Value
                    : string.Empty,
                Method = context.Request.Method,
                Headers = context.Request.Headers,
                Body = bodyText
            };

            var json = JsonSerializer.Serialize(
                logObject,
                new JsonSerializerOptions
                {
                    WriteIndented = false
                });

            _logger.LogInformation("IncomingHttpRequest: {Request}", json);

            await _next(context).ConfigureAwait(false);
        }
    }
}