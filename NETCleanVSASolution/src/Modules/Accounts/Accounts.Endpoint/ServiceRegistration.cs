using Framework.Infrastructure;

namespace Accounts.Endpoint
{
    public static class ServiceRegistrationAccountsEndpointExtensions
    {
        public static IServiceCollection AddAccountsEndpoint(this IServiceCollection services, IConfiguration configuration)
        {
            //Register Account Application Services
            services.AddFrameworkInfrastructure();
            services.AddFrameworkApplication();
            services.AddAccountsApplication(configuration);
            services.AddAccountsDatabase(configuration);

            return services;
        }
    }
}
