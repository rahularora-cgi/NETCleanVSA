namespace Users.Persistence
{
    public static class UsersPersistenceExtensions
    {
        public static IServiceCollection AddUsersDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UsersDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(UsersDbSettings.ConnectionStringKey));
            });

            services.AddScoped<IUsersDbContext>(sp => sp.GetRequiredService<UsersDbContext>());

            return services;
        }
    }

}

