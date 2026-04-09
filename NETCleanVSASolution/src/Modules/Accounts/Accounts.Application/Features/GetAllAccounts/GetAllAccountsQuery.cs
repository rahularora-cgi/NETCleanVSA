using Framework.Application.Abstractions.CQRS;

namespace Accounts.Application.Features.GetAllAccounts
{
    public record GetAllAccountsQuery() : IQuery<GetAccountListDto>;
}