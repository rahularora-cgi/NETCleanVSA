namespace Framework.Application.Abstractions.Events
{
    /// <summary>
    /// Dispatcher for publishing domain events to registered handlers.
    /// Follows the same pattern as ICommandDispatcher and IQueryDispatcher.
    /// </summary>
    public interface IDomainEventDispatcher
    {
        /// <summary>
        /// Publishes a domain event to all registered handlers.
        /// Multiple handlers can handle the same event.
        /// </summary>
        Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
            where TDomainEvent : IDomainEvent;
    }
}