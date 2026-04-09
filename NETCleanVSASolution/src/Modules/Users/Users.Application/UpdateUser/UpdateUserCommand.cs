using Framework.Application.Abstractions.CQRS;

namespace Users.Application.UpdateUser
{
    public record UpdateUserCommand(
        Guid Id,
        string? FirstName,
        string? LastName,
        string? UserName,
        string? Email,
        string? Password,
        bool? IsActive
    ) : ICommand<Unit>;
}