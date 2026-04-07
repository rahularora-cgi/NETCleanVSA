namespace Framework.Application
{
    public static class ServiceRegistration
    {
        public static void AddFrameworkApplication(this IServiceCollection services)
        {
            //Register Dispathers
            services.TryAddScoped<ICommandDispatcher, CommandDispatcher>();
            services.TryAddScoped<IQueryDispatcher, QueryDispatcher>();
            services.TryAddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        }
    }
}
