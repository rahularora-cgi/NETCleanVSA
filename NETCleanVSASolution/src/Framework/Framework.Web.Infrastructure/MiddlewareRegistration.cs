namespace Framework.Infrastructure.Web
{
    public static class MiddlewareRegistration
    {
        public static IApplicationBuilder RegisterMiddleware(this IApplicationBuilder app)
        {
            app.UseHttpLogging();
            return app;
        }
    }
}
