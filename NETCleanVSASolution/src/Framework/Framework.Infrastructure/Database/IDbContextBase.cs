namespace Framework.Infrastructure.Database
{
    internal interface IDbContextBase
    {
        DbSet<OutboxMessage> OutBoxMessages { get; set; }
        DbSet<OutboxMessageConsumer> OutBoxMessageConsumer { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
