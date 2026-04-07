using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Users.Presentation;
using Accounts.Presentation;
using Framework.Infrastructure.Web;
using Framework.Infrastructure.Notifications;

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
            services.AddUsersPresentation(configuration);
            services.AddAccountsPresentation(configuration);
            return services;
        }
    }
}
