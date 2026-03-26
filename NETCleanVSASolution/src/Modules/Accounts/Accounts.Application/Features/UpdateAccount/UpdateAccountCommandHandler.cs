namespace Accounts.Application.Features.UpdateAccount
{
    public class UpdateAccountCommandHandler(AccountsDbContext _dbContext, ILogger<UpdateAccountCommandHandler> _logger)
        : ICommandHandler<UpdateAccountCommand, Unit>
    {
        public async Task<Result<Unit>> HandleAsync(UpdateAccountCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating account with ID: {AccountId}", command.Id);

                // Find the account
                var account = await _dbContext.Accounts
                    .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);

                if (account == null)
                {
                    return Result.Failure<Unit>(
                        Error.NotFound("Account", command.Id));
                }

                // Check if account number is being changed and if it already exists
                if (!string.IsNullOrWhiteSpace(command.AccountNumber) && 
                    command.AccountNumber != account.AccountNumber)
                {
                    bool accountNumberExists = await _dbContext.Accounts
                        .AnyAsync(a => a.AccountNumber == command.AccountNumber, cancellationToken);

                    if (accountNumberExists)
                    {
                        return Result.Failure<Unit>(
                            Error.Conflict($"Account with number '{command.AccountNumber}' already exists"));
                    }

                    account.AccountNumber = command.AccountNumber;
                }

                // Update account name if provided
                if (!string.IsNullOrWhiteSpace(command.AccountName))
                {
                    account.AccountName = command.AccountName;
                }

                // Update account owner if provided
                if (command.AccountOwner != null)
                {
                    account.AccountOwner = string.IsNullOrWhiteSpace(command.AccountOwner) 
                        ? null 
                        : command.AccountOwner;
                }

                // Update phone if provided
                if (!string.IsNullOrWhiteSpace(command.Phone))
                {
                    account.Phone = command.Phone;
                }

                // Update fax if provided
                if (command.Fax != null)
                {
                    account.Fax = string.IsNullOrWhiteSpace(command.Fax) 
                        ? null 
                        : command.Fax;
                }

                // Update website if provided
                if (command.Website != null)
                {
                    account.Website = string.IsNullOrWhiteSpace(command.Website) 
                        ? null 
                        : command.Website;
                }

                // Update billing address if provided
                if (!string.IsNullOrWhiteSpace(command.BillingAddress))
                {
                    account.BillingAddress = command.BillingAddress;
                }

                // Update shipping address if provided
                if (!string.IsNullOrWhiteSpace(command.ShippingAddress))
                {
                    account.ShippingAddress = command.ShippingAddress;
                }

                // Update type if provided
                if (!string.IsNullOrWhiteSpace(command.Type))
                {
                    account.Type = command.Type;
                }

                // Update industry if provided
                if (command.Industry != null)
                {
                    account.Industry = string.IsNullOrWhiteSpace(command.Industry) 
                        ? null 
                        : command.Industry;
                }

                // Update annual revenue if provided
                if (command.AnnualRevenue.HasValue)
                {
                    account.AnnualRevenue = command.AnnualRevenue.Value;
                }

                // Update number of employees if provided
                if (command.NumberOfEmployees.HasValue)
                {
                    account.NumberOfEmployees = command.NumberOfEmployees.Value;
                }

                // Update ownership if provided
                if (command.Ownership != null)
                {
                    account.Ownership = string.IsNullOrWhiteSpace(command.Ownership) 
                        ? null 
                        : command.Ownership;
                }

                account.ModifiedAtUtc = DateTime.UtcNow;

                _dbContext.Accounts.Update(account);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Account updated successfully with ID: {AccountId}", account.Id);

                return Result.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating account with ID: {AccountId}", command.Id);
                return Result.Failure<Unit>(Error.Custom("Account.UpdateFailed", ex.Message));
            }
        }
    }
}