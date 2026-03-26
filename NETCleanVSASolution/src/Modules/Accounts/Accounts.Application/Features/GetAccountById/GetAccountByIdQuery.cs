namespace Accounts.Application.Features.GetAccountById
{
    public record GetAccountByIdQuery(int Id) : IQuery<GetAccountDto>;
   
}
