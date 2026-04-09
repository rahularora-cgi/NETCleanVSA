using Framework.Application.Abstractions.CQRS;

namespace Users.Application.QueryUser
{
    public record GetAllUsersQuery() : IQuery<IEnumerable<GetUserDto>>;
}