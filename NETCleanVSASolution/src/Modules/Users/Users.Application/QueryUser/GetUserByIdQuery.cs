using Framework.Application.Abstractions.CQRS;

namespace Users.Application.QueryUser
{
    public record GetUserByIdQuery(Guid id) : IQuery<GetUserDto>;
}