using Framework.Application.Abstractions.CQRS;

namespace Users.Application.QueryRole
{
    public record GetRolesByUserIdQuery(Guid userId) : IQuery<IEnumerable<GetRoleDto>>;
   
}
