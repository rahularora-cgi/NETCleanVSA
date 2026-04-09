using Framework.Application.Abstractions.CQRS;

namespace Users.Application.QueryUser
{
    public record GetAllUsersPaginatedQuery(
        int PageNumber = 1,
        int PageSize = 50,
        bool? IsActive = null
    ) : IQuery<PagedResult<GetUserDto>>;
}
