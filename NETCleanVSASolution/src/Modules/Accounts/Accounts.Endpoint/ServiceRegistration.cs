using Framework.Infrastructure;

namespace Accounts.Endpoint
{
    public static class AccountsEndpointExtensions
    {
        public static IServiceCollection AddAccountsEndpoint(this IServiceCollection services, IConfiguration configuration)
        {
            //Register Account Application Services
            services.AddAccountsApplication(configuration);
            services.AddAccountsDatabase(configuration);

            return services;
        }
    }
}
