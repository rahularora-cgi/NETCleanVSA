namespace Framework.Infrastructure.Database
{
    public abstract class DbContextBase(DbContextOptions options) : DbContext(options), IDbContextBase
    {
        public DbSet<OutboxMessage> OutBoxMessages { get; set; }
        public DbSet<OutboxMessageConsumer> OutBoxMessageConsumer { get; set; }

        //Todo: Revisit this
        //this can be done via an interceptor as well, but for simplicity, we will do it here.
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<IEntityBase>().Where(entry => entry.State == EntityState.Added
                                    || entry.State == EntityState.Modified);

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Entity.CreatedAtUtc = DateTime.UtcNow;
                    entity.Entity.ModifiedAtUtc = DateTime.UtcNow;
                }

                if (entity.State == EntityState.Modified)
                {
                    entity.Entity.ModifiedAtUtc = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
