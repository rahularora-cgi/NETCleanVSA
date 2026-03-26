namespace Users.Application.QueryRole
{
    public record GetRolesByUserIdQuery(Guid userId) : IQuery<IEnumerable<GetRoleDto>>;
   
}
