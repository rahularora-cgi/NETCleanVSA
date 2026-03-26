namespace Framework.Domain
{
    internal interface IDomainEventHandler<T> where T : IDomainEvent
    {
        Task Handle(T domainEvent, CancellationToken cancellationToken);
    }
}
