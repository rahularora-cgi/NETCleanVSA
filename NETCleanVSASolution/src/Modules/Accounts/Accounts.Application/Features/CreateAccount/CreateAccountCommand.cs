using Framework.Application.Abstractions.CQRS;

namespace Accounts.Application.Features.CreateAccount
{
    //public record CreateAccountCommand(string accountName, string desc): ICommand;

    public record CreateAccountCommand(string AccountName, string AccountOwner) : ICommand<int>;

}
