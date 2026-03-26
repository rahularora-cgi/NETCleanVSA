namespace Framework.Domain
{
    public interface IDomainEvent
    {
        Guid EventId { get; }
        DateTime OccurredAtUtc { get; }
    }
}

public abstract record DomainEventBase : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;
}