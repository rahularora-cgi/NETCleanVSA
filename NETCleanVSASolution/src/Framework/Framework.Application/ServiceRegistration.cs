using Framework.Application.Abstractions.CQRS;
using Framework.Application.Abstractions.Events;

namespace Framework.Application
{
    public static class ServiceRegistrationFrameworkApplicationExtensions
    {
        public static IServiceCollection AddFrameworkApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
