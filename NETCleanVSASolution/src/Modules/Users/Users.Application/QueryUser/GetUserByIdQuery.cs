namespace Users.Application.QueryUser
{
    public record GetUserByIdQuery(Guid id) : IQuery<GetUserDto>;
}