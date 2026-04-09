using Framework.Application.Abstractions.CQRS;
using Framework.Application.Abstractions.Events;
using Framework.Infrastructure.CQRS;

namespace Framework.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddFrameworkInfrastructure(this IServiceCollection services)
        {
            services.AddCQRS();
            return services;
        }
    }
}
