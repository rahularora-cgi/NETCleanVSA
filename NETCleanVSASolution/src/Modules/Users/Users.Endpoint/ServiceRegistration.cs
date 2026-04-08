using Users.Infrastructure;
using Users.Persistence;

namespace Users.Endpoint
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddUsersEndpoint(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddUsersApplication(configuration);
            services.AddUsersInfrastructure(configuration);
            services.AddUsersDatabase(configuration);
            return services;
        }
    }
}
