namespace Accounts.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddAccountsDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AccountsDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(AccountsDbSettings.ConnectionStringKey));
            });

            return services;
        }
    }
}
