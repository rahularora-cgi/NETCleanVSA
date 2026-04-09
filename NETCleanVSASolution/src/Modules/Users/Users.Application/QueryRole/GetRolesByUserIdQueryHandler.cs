using Framework.Application.Abstractions.CQRS;

namespace Users.Application.QueryRole
{
    public class GetRolesByUserIdQueryHandler(IUsersDbContext _dbContext, ILogger<GetRolesByUserIdQueryHandler> _logger)
        : IQueryHandler<GetRolesByUserIdQuery, IEnumerable<GetRoleDto>>
    {
        public async Task<Result<IEnumerable<GetRoleDto>>> HandleAsync(GetRolesByUserIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching roles for user with ID: {UserId}", query.userId);

                var roles = await _dbContext.UserRoles
                    .AsNoTracking()
                    .Include(ur => ur.Role)
                    .Where(ur => ur.UserId == query.userId)
                    .Select(ur => new GetRoleDto(
                        ur.Role.Id,
                        ur.Role.RoleName))
                    .ToListAsync(cancellationToken);

                return Result.Success(roles as IEnumerable<GetRoleDto>);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with Id: {UserId}", query.userId);
                return Result.Failure<IEnumerable<GetRoleDto>>(Error.Custom("User.FetchFailed", ex.Message));
            }
        }
    }
}


