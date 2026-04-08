namespace Users.Application.UpdateUser
{
    public class UpdateUserCommandHandler(IUsersDbContext _dbContext, ILogger<UpdateUserCommandHandler> _logger)
        : ICommandHandler<UpdateUserCommand, Unit>
    {
        public async Task<Result<Unit>> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating user with ID: {UserId}", command.Id);

                // Find the user
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);

                if (user == null)
                {
                    return Result.Failure<Unit>(
                        Error.NotFound("User", command.Id));
                }

                // Check if email is being changed and if it already exists
                if (!string.IsNullOrWhiteSpace(command.Email) && command.Email != user.Email)
                {
                    bool isEmailExists = await _dbContext.Users
                        .AnyAsync(u => u.Email.ToLower() == command.Email.ToLower(), cancellationToken);

                    if (isEmailExists)
                    {
                        return Result.Failure<Unit>(
                            Error.Conflict($"User with email '{command.Email}' already exists"));
                    }

                    user.Email = command.Email;
                }

                // Update first name if provided
                if (!string.IsNullOrWhiteSpace(command.FirstName))
                {
                    user.FirstName = command.FirstName;
                }

                // Update last name if provided
                if (!string.IsNullOrWhiteSpace(command.LastName))
                {
                    user.LastName = command.LastName;
                }

                // Update username if provided
                if (!string.IsNullOrWhiteSpace(command.UserName))
                {
                    user.UserName = command.UserName;
                }

                // Update password if provided
                if (!string.IsNullOrWhiteSpace(command.Password))
                {
                    var passwordHasher = new PasswordHasher<User>();
                    user.PasswordHash = passwordHasher.HashPassword(user, command.Password);
                }

                // Update IsActive if provided
                if (command.IsActive.HasValue)
                {
                    user.IsActive = command.IsActive.Value;
                    
                    _logger.LogInformation("User {UserId} active status changed to: {IsActive}", 
                        user.Id, user.IsActive);
                }

                user.ModifiedAtUtc = DateTime.UtcNow;

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("User updated successfully with ID: {UserId}", user.Id);

                return Result.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID: {UserId}", command.Id);
                return Result.Failure<Unit>(Error.Custom("User.UpdateFailed", ex.Message));
            }
        }
    }
}