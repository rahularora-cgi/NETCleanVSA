using Accounts.Application;
using Framework.Application.Abstractions.Events;

namespace Accounts.Persistence.Database
{
    public class AccountsDbContext(DbContextOptions<AccountsDbContext> options, IDomainEventDispatcher domainEventDispatcher) : DbContextBase(options, domainEventDispatcher), IAccountsDbContext
    {
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
             .ToTable("Accounts");
          
        }
    }
}
