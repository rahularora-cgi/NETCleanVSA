using Microsoft.EntityFrameworkCore;

namespace Framework.Application.Abstractions.Database
{
    public interface IDbContextBase
    {
        //DbSet<OutboxMessage> OutBoxMessages { get; set; }
        //DbSet<OutboxMessageConsumer> OutBoxMessageConsumer { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
