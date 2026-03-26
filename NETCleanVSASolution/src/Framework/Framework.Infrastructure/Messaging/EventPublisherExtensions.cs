
namespace Framework.Infrastructure.Messaging    
{
    public static class EventPublisherExtensions
    {
        public static IServiceCollection AddEventPublisher(this IServiceCollection services, IConfiguration configuration) 
        {
            //Microsoft.Extensions.Options.OptionsBuilder<MessageBrokerSettings> b; b.bind

            services.AddOptions<MessageBrokerSettings>().Bind(configuration.GetSection(nameof(MessageBrokerSettings)));

            services.AddSingleton<IEventPublisher, EventPublisher>();
            
            return services;
        }
    }
}
