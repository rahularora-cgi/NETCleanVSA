namespace Accounts.Persistence.Database
{
    public class AccountsDbContext(DbContextOptions<AccountsDbContext> options): DbContextBase(options)
    {
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
             .ToTable("Accounts");
          
        }
    }
}
