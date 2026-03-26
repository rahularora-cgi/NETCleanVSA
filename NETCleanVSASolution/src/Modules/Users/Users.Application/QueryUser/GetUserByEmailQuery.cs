namespace Users.Application.QueryUser
{
    public record GetUserByEmailQuery(string email) : IQuery<GetUserDto>;
   
}
