namespace Users.Application
{
    public static class ServiceRegistrationUsersApplication
    {
        public static IServiceCollection AddUsersApplication(this IServiceCollection services, IConfiguration configuration)
        {
            //Register Framework Application Services
            services.AddFrameworkApplication();

            services.AddUsersInfrastructure(configuration);
            services.AddUserDatabase(configuration);

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

            // Register validators
            services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();


            return services;

        }
    }
}
