using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Framework.Infrastructure.Web;
using Framework.Infrastructure.Notifications;
using Users.Endpoint;
using Accounts.Endpoint;

namespace CRM.Web.Api
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCRMApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddNotificationServices(configuration);

            //Register Framework Infrastructure
            services.AddFrameworkWebInfrastructure(configuration);

            //Register Account Application Services
            services.AddUsersEndpoint(configuration);
            services.AddAccountsEndpoint(configuration);
            return services;
        }
    }
}
