namespace Users.Application.QueryUser
{
    public class GetAllUsersQueryHandler(UsersDbContext _dbContext, ILogger<GetAllUsersQueryHandler> _logger)
        : IQueryHandler<GetAllUsersQuery, IEnumerable<GetUserDto>>
    {
        public async Task<Result<IEnumerable<GetUserDto>>> HandleAsync(GetAllUsersQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching all users");

                var users = await _dbContext.Users
                    .AsNoTracking()
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Select(u => new GetUserDto(
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.UserName,
                        u.Email,
                        u.PasswordHash,
                        u.IsActive,
                        u.CreatedAtUtc,
                        u.ModifiedAtUtc
                    ))
                    .ToListAsync(cancellationToken);

                _logger.LogInformation("Successfully fetched {Count} users", users.Count);

                return Result.Success<IEnumerable<GetUserDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users");
                return Result.Failure<IEnumerable<GetUserDto>>(
                    Error.Custom("User.FetchAllFailed", ex.Message));
            }
        }
    }
}