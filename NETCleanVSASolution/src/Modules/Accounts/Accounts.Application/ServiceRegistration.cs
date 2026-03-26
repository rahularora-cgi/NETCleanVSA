using Accounts.Persistence;

namespace Accounts.Application
{
    public static class ServiceRegistrationAccountsApplication
    {
        public static IServiceCollection AddAccountsApplication(this IServiceCollection services, IConfiguration configuration)
        {
            //Register Framework Application Services
            services.AddFrameworkApplication();

            services.AddAccountsDatabase(configuration);

            //Register Command Handlers
            services.AddScoped<ICommandHandler<CreateAccountCommand, int>, CreateAccountCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateAccountCommand, Unit>, UpdateAccountCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteAccountCommand, Unit>, DeleteAccountCommandHandler>();

            //Register Query Handlers
            services.AddScoped<IQueryHandler<GetAccountByIdQuery, GetAccountDto>, GetAccountByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetAllAccountsQuery, GetAccountListDto>, GetAllAccountsQueryHandler>();

            //Register Validators
            //services.AddScoped<IValidator<CreateAccountCommand>, CreateAccountValidator>();

            services.AddValidatorsFromAssembly(typeof(ServiceRegistrationAccountsApplication).Assembly);

            //To use the Decorator Pattern for logging, we need to register the concrete handler first, then the decorator

            //services.AddTransient<CreateAccountCommandHandler>();  

            // 3. Register the interface to resolve as the Decorator, passing the concrete handler inside
            //services.AddTransient<ICommandHandler<CreateAccountCommand, int>>(serviceProvider =>
            //{
            //    // Resolve the real handler
            //    var innerHandler = serviceProvider.GetRequiredService<CreateAccountCommandHandler>();

            //    // Wrap it in the logging decorator
            //    return new CommandHandlerLoggingDecorator<CreateAccountCommand, int>(innerHandler);
            //});

            return services;

        }
    }
}
