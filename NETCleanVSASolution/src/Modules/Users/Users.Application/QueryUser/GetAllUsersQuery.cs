namespace Users.Application.QueryUser
{
    public record GetAllUsersQuery() : IQuery<IEnumerable<GetUserDto>>;
}