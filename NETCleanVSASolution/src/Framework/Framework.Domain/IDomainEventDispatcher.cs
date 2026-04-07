namespace Framework.Domain
{
    public interface IDomainEventDispatcher
    {
        Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
            where TDomainEvent : IDomainEvent;
    }
}
