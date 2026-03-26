namespace Users.Application.CreateUser
{
    public class CreateUserCommandHandler(UsersDbContext _dbContext, ILogger<CreateUserCommandHandler> _logger) 
        : ICommandHandler<CreateUserCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Creating user with email: {Email}", command.Email);

                // Check if user already exists
                bool isUserExist = await _dbContext.Users
                    .AnyAsync(user => user.Email == command.Email, cancellationToken);

                if (isUserExist)
                {
                    return Result.Failure<Guid>(
                        Error.Conflict($"User with email '{command.Email}' already exists"));
                }

                // Hash the password
                var user = new User
                {
                    //Id = Guid.NewGuid(), db generates it
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    UserName = command.UserName,
                    IsActive = true,
                    Email = command.Email
                };

                var passwordHasher = new PasswordHasher<User>();
                user.PasswordHash = passwordHasher.HashPassword(user, command.Password);

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);

                return Result.Success(user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with email: {Email}", command.Email);
                return Result.Failure<Guid>(Error.Custom("User.CreateFailed", ex.Message));
            }
        }
    }
}