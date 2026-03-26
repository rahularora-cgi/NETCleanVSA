namespace Users.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddJWTAuthentication(configuration);

            return services;
        }
    }
}
