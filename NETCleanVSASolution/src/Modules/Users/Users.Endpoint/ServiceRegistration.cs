namespace Users.Presentation
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddUsersPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            //Register Users Application Services
            services.AddUsersApplication(configuration);
            return services;
        }
    }
}
