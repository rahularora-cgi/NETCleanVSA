namespace Users.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddUserDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UsersDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(UsersDbSettings.ConnectionStringKey));
            });

            return services;
        }
    }

}

