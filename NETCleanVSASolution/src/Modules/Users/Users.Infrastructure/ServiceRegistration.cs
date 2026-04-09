using Microsoft.AspNetCore.Http;

namespace Users.Infrastructure
{
    public static class ServiceRegistrationUsersUsersInfrastructureExtensions
    {
        public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddJWTAuthentication(configuration);

            return services;
        }
    }
}
