namespace Accounts.Presentation
{
    public static class ServiceRegistrationUsersApplication
    {
        public static IServiceCollection AddAccountsPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            //Register Account Application Services
            services.AddAccountsApplication(configuration);
            return services;
        }
    }
}
