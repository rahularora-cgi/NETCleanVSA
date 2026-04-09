using Accounts.Application;

namespace Accounts.Persistence
{
    public static class ServiceRegistrationAccountsPersistenceExtensions
    {
        public static IServiceCollection AddAccountsDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AccountsDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(AccountsDbSettings.ConnectionStringKey));
            });

            services.AddScoped<IAccountsDbContext>(sp => sp.GetRequiredService<AccountsDbContext>());

            return services;
        }
    }
}
