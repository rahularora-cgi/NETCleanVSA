namespace Framework.Infrastructure.Notifications
{
    public static class NotificationExtensions
    {
        public static IServiceCollection AddNotificationServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddScoped<INotificationService<EmailNotificationMessage>, EmailNotificationService>();
            services.AddScoped<INotificationService<TextNotificationMessage>, TextNotificationService>();

            return services;
        }
    }
}
