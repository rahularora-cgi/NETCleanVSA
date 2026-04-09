using Framework.Application.Abstractions.CQRS;

namespace Users.Application.QueryUser
{
    public record GetUserByEmailQuery(string email) : IQuery<GetUserDto>;
   
}
