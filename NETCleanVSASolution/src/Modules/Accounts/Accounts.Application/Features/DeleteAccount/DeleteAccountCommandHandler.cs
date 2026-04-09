using Framework.Application.Abstractions.CQRS;

namespace Accounts.Application.Features.DeleteAccount
{
    public class DeleteAccountCommandHandler(IAccountsDbContext _dbContext, ILogger<DeleteAccountCommandHandler> _logger)
        : ICommandHandler<DeleteAccountCommand, Unit>
    {
        public async Task<Result<Unit>> HandleAsync(DeleteAccountCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting account with ID: {AccountId}", command.Id);

                // Find the account
                var account = await _dbContext.Accounts
                    .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);

                if (account == null)
                {
                    return Result.Failure<Unit>(
                        Error.NotFound("Account", command.Id));
                }

                _dbContext.Accounts.Remove(account);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Account deleted successfully with ID: {AccountId}", command.Id);

                return Result.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account with ID: {AccountId}", command.Id);
                return Result.Failure<Unit>(Error.Custom("Account.DeleteFailed", ex.Message));
            }
        }
    }
}