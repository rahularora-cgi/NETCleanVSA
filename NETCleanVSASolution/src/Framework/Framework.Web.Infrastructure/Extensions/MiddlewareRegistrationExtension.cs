namespace Framework.Infrastructure.Web.Extensions
{
    public static class MiddlewareRegistrationExtension
    {
        public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpRequestLoggingMiddleware>();
        }
    }
}
