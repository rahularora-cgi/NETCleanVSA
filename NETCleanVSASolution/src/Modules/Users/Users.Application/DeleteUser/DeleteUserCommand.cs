namespace Users.Application.DeleteUser
{
    public record DeleteUserCommand(Guid Id) : ICommand<Unit>;
}