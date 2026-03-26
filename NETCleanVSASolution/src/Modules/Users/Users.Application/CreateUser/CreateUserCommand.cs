namespace Users.Application.CreateUser
{
    public record CreateUserCommand(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string Password,
        bool IsActive
    ) : ICommand<Guid>;
}