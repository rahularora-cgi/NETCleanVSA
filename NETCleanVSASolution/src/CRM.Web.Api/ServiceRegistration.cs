using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Users.Endpoint;
using Accounts.Endpoint;
using Framework.Application;
using Framework.Infrastructure;
using Framework.Infrastructure.Web;
using Framework.Infrastructure.Notifications;

namespace CRM.Web.Api
{
    public static class ServiceRegistrationCRMWebApiExtensions
    {
        public static IServiceCollection AddCRMApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Register Framework Infrastructure
            services.AddFrameworkApplication();
            services.AddFrameworkInfrastructure();
            services.AddFrameworkWebInfrastructure(configuration);

            services.AddNotificationServices(configuration);

            //Register Account Application Services
            services.AddUsersEndpoint(configuration);
            services.AddAccountsEndpoint(configuration);
            return services;
        }
    }
}
