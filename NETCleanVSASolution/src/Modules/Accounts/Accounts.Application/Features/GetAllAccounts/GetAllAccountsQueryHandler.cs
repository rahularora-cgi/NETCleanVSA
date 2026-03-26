namespace Accounts.Application.Features.GetAllAccounts
{
    public class GetAllAccountsQueryHandler(
        AccountsDbContext _dbContext,
        ILogger<GetAllAccountsQueryHandler> _logger)
        : IQueryHandler<GetAllAccountsQuery, GetAccountListDto>
    {
        public async Task<Result<GetAccountListDto>> HandleAsync(
            GetAllAccountsQuery query, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching all accounts");

                var accounts = await _dbContext.Accounts
                    .AsNoTracking()
                    .OrderBy(a => a.AccountName)
                    .Select(a => new GetAccountListItemDto(
                        a.Id,
                        a.AccountName,
                        a.AccountNumber,
                        a.AccountOwner,
                        a.Phone,
                        a.Fax,
                        a.Website,
                        a.BillingAddress,
                        a.ShippingAddress,
                        a.Type,
                        a.Industry,
                        a.AnnualRevenue,
                        a.NumberOfEmployees,
                        a.Ownership,
                        a.CreatedAtUtc,
                        a.ModifiedAtUtc
                    ))
                    .ToListAsync(cancellationToken);

                _logger.LogInformation("Successfully fetched {Count} accounts", accounts.Count);

                return Result.Success(new GetAccountListDto { Accounts = accounts });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all accounts");
                return Result.Failure<GetAccountListDto>(
                    Error.Custom("Account.FetchAllFailed", ex.Message));
            }
        }
    }
}