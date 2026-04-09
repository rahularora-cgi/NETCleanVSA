using Framework.Application.Abstractions.CQRS;
using Framework.Application.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Infrastructure.CQRS
{
    public static class CQRSExtensions
    {
        public static void AddCQRS(this IServiceCollection services)
        {
            //Register Dispathers
            services.TryAddScoped<ICommandDispatcher, CommandDispatcher>();
            services.TryAddScoped<IQueryDispatcher, QueryDispatcher>();
            services.TryAddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        }
    }
}
