using Framework.Application.Abstractions.CQRS;

namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountCommandHandler(IAccountsDbContext _dbContext, ILogger<CreateAccountCommandHandler> _logger) : ICommandHandler<CreateAccountCommand, int>
    {
        public async Task<Result<int>> HandleAsync(CreateAccountCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                //var IsValid = _validator.Validate(command);

                _logger.LogInformation($"[Database] Creating account: {command.AccountName}");

                var account = new Account
                {
                    AccountName = command.AccountName,
                    AccountOwner = command.AccountOwner
                };

                _dbContext.Accounts.Add(account);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Result.Success(account.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account");
                return Result.Failure<int>(new Error("Account.CreateFailed", ex.Message));
            }
        }
    }
}
