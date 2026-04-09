using Framework.Application.Abstractions.CQRS;

namespace Accounts.Application.Features.DeleteAccount
{
    public record DeleteAccountCommand(int Id) : ICommand<Unit>;
}