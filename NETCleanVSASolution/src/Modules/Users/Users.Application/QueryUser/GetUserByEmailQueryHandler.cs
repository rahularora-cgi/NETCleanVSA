namespace Users.Application.QueryUser
{
    public class GetUserByEmailQueryHandler(IUsersDbContext _dbContext, ILogger<GetUserByEmailQueryHandler> _logger) : IQueryHandler<GetUserByEmailQuery, GetUserDto>
    {
        public async Task<Result<GetUserDto>> HandleAsync(GetUserByEmailQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching user with email: {UserEmail}", query.email);

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == query.email, cancellationToken);
                if (user == null)
                {
                    _logger.LogWarning("User with email: {UserEmail} not found", query.email);
                    return Result.Failure<GetUserDto>(Error.NotFound("User", query.email));
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
                _logger.LogError(ex, "Error fetching user with email: {UserEmail}", query.email);
                return Result.Failure<GetUserDto>(Error.Custom("User.FetchFailed", ex.Message));
            }
        }

    }
}
