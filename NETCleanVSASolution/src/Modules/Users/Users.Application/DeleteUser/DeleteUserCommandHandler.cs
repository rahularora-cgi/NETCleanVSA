using Framework.Application.Abstractions.CQRS;

namespace Users.Application.DeleteUser
{
    public class DeleteUserCommandHandler(IUsersDbContext _dbContext, ILogger<DeleteUserCommandHandler> _logger)
        : ICommandHandler<DeleteUserCommand, Unit>
    {
        public async Task<Result<Unit>> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID: {UserId}", command.Id);

                // Find the user
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);

                if (user == null)
                {
                    return Result.Failure<Unit>(
                        Error.NotFound("User", command.Id));
                }

                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("User deleted successfully with ID: {UserId}", command.Id);

                return Result.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID: {UserId}", command.Id);
                return Result.Failure<Unit>(Error.Custom("User.DeleteFailed", ex.Message));
            }
        }
    }
}