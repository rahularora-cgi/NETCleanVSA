namespace Framework.Infrastructure.Web.OpenApi
{
    public static class ScalarApiExtension
    {
        public static IEndpointRouteBuilder MapScalarForApiDocs(this IEndpointRouteBuilder app)
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
            app.MapGet("/", () => Results.Redirect("/scalar"));

            return app;
        }

    }
}
