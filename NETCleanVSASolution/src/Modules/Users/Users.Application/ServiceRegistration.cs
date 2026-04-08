namespace Users.Application
{
    public static class ServiceRegistrationUsersApplication
    {
        public static IServiceCollection AddUsersApplication(this IServiceCollection services, IConfiguration configuration)
        {
            //Register Framework Application Services
            services.AddFrameworkApplication();

            //Register Query Handlers
            services.AddScoped<IQueryHandler<GetRolesByUserIdQuery, IEnumerable<GetRoleDto>>, GetRolesByUserIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetUserByEmailQuery, GetUserDto>, GetUserByEmailQueryHandler>();
            services.AddScoped<IQueryHandler<GetUserByIdQuery, GetUserDto>, GetUserByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetAllUsersQuery, IEnumerable<GetUserDto>>, GetAllUsersQueryHandler>();
            services.AddScoped<IQueryHandler<GetAllUsersPaginatedQuery, PagedResult<GetUserDto>>, GetAllUsersPaginatedQueryHandler>();

            //Register Command Handlers
            services.AddScoped<ICommandHandler<CreateUserCommand, Guid>, CreateUserCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateUserCommand, Unit>, UpdateUserCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteUserCommand, Unit>, DeleteUserCommandHandler>();

            //Register Services
            services.AddScoped<ILoginService, LoginService>();

            // Register Event Consumers (for Outbox pattern processing)
            services.AddScoped<UserCreatedEventConsumer>();

            // Register validators
            services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();

            //Register Domain Event Handlers (for in-memory event processing)
            services.AddScoped<IDomainEventHandler<UserCreatedDomainEvent>, UserCreatedDomainEventHandler>();

            return services;

        }
    }
}
