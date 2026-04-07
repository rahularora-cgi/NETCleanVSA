using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Framework.Infrastructure.Database
{
    public abstract class DbContextBase(DbContextOptions options, IDomainEventDispatcher? domainEventDispatcher = null)
        : DbContext(options), IDbContextBase
    {
        private readonly IDomainEventDispatcher? _domainEventDispatcher = domainEventDispatcher;

        public DbSet<OutboxMessage> OutBoxMessages { get; set; }
        public DbSet<OutboxMessageConsumer> OutBoxMessageConsumer { get; set; }

        //Todo: Revisit this
        //this can be done via an interceptor as well, but for simplicity, we will do it here.
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
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

            await PublishDomainEvents(cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task PublishDomainEvents(CancellationToken cancellationToken)
        {
            var aggregateRoots = ChangeTracker
                .Entries()
                .Select(e => e.Entity)
                .OfType<IAggregateRoot>()
                .Where(e => e.DomainEvents.Any())
                .ToList();

            var domainEvents = aggregateRoots
                .SelectMany(aggregateRoot => aggregateRoot.DomainEvents)
                .ToList();

            aggregateRoots.ForEach(aggregateRoot => aggregateRoot.ClearDomainEvents());

            //To use in-memory dispatch, simply inject IDomainEventDispatcher into your derived DbContext:

            if (_domainEventDispatcher is not null)
            {
                // In-memory: dispatch immediately to registered IDomainEventHandler<T> handlers
                foreach (var domainEvent in domainEvents)
                {
                    await _domainEventDispatcher.PublishAsync((dynamic)domainEvent, cancellationToken);
                }
            }
            else
            {
                //To use the Outbox pattern (default), omit the dispatcher:
                // Outbox pattern: serialize events to the database for background processing
                foreach (var domainEvent in domainEvents)
                {
                    var outboxMessage = new OutboxMessage
                    {
                        Id = Guid.NewGuid(),
                        OccurredAtUtc = DateTime.UtcNow,
                        Type = domainEvent.GetType().Name,
                        Content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType())
                    };

                    await OutBoxMessages.AddAsync(outboxMessage, cancellationToken);
                }
            }
        }

    }
}
