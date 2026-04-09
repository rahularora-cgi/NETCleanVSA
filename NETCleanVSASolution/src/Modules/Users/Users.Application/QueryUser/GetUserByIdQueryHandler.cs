using Framework.Application.Abstractions.CQRS;

namespace Users.Application.QueryUser
{
    public class GetUserByIdQueryHandler(IUsersDbContext _dbContext, ILogger<GetUserByIdQueryHandler> _logger) 
        : IQueryHandler<GetUserByIdQuery, GetUserDto>
    {
        public async Task<Result<GetUserDto>> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID: {UserId}", query.id);

                var user = await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == query.id, cancellationToken);

                if (user == null)
                {
                    _logger.LogWarning("User with ID: {UserId} not found", query.id);
                    return Result.Failure<GetUserDto>(Error.NotFound("User", query.id));
                }

                var dtoUser = new GetUserDto(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.UserName,
                    user.Email,
                    user.PasswordHash,
                    user.IsActive,
                    user.CreatedAtUtc,
                    user.ModifiedAtUtc
                );

                return Result.Success(dtoUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with ID: {UserId}", query.id);
                return Result.Failure<GetUserDto>(Error.Custom("User.FetchFailed", ex.Message));
            }
        }
    }
}