namespace Users.Application.QueryUser
{
    public class GetAllUsersPaginatedQueryHandler(UsersDbContext _dbContext, ILogger<GetAllUsersPaginatedQueryHandler> _logger)
        : IQueryHandler<GetAllUsersPaginatedQuery, PagedResult<GetUserDto>>
    {
        public async Task<Result<PagedResult<GetUserDto>>> HandleAsync(GetAllUsersPaginatedQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching users - Page: {PageNumber}, Size: {PageSize}, IsActive: {IsActive}",
                    query.PageNumber, query.PageSize, query.IsActive);

                var queryable = _dbContext.Users.AsNoTracking();

                // Apply filter
                if (query.IsActive.HasValue)
                {
                    queryable = queryable.Where(u => u.IsActive == query.IsActive.Value);
                }

                // Get total count
                var totalCount = await queryable.CountAsync(cancellationToken);

                // Get paged data
                var users = await queryable
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
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

                var pagedResult = new PagedResult<GetUserDto>
                {
                    Items = users,
                    TotalCount = totalCount,
                    PageNumber = query.PageNumber,
                    PageSize = query.PageSize
                };

                _logger.LogInformation(
                    "Successfully fetched {Count} of {Total} users",
                    users.Count, totalCount);

                return Result.Success(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users");
                return Result.Failure<PagedResult<GetUserDto>>(
                    Error.Custom("User.FetchAllFailed", ex.Message));
            }
        }
    }
}