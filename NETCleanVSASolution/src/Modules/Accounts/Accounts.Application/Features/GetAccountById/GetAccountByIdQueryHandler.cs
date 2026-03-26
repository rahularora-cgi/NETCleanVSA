namespace Accounts.Application.Features.GetAccountById
{
    public class GetAccountByIdQueryHandler(AccountsDbContext _dbContext, ILogger<GetAccountByIdQueryHandler> _logger) : IQueryHandler<GetAccountByIdQuery, GetAccountDto>
    {
        public async Task<Result<GetAccountDto>> HandleAsync(GetAccountByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching account with ID: {AccountId}", query.Id);

                var account = await _dbContext.Accounts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == query.Id, cancellationToken);

                if (account == null)
                {
                    _logger.LogWarning("Account with ID {AccountId} not found", query.Id);
                    return Result.Failure<GetAccountDto>(Error.NotFound("Account", query.Id));
                }

                var dto = new GetAccountDto(
                    account.Id,
                    account.AccountName,
                    account.AccountNumber,
                    account.AccountOwner,
                    account.Phone,
                    account.Fax,
                    account.Website,
                    account.BillingAddress,
                    account.ShippingAddress,
                    account.Type,
                    account.Industry,
                    account.AnnualRevenue,
                    account.NumberOfEmployees,
                    account.Ownership,
                    account.CreatedAtUtc,
                    account.ModifiedAtUtc
                );

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching account with ID: {AccountId}", query.Id);
                return Result.Failure<GetAccountDto>(Error.Custom("Account.FetchFailed", ex.Message));
            }
        }
    }
}
