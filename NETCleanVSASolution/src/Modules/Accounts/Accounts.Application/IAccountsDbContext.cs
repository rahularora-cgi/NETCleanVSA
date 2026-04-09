namespace Accounts.Application
{
    public interface IAccountsDbContext
    {
        DbSet<Account> Accounts { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
