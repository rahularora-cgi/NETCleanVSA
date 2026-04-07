namespace Framework.Application
{
    /// <summary>
    /// Handler interface for domain events.
    /// Multiple handlers can be registered for the same domain event.
    /// Follows the same pattern as ICommandHandler and IQueryHandler.
    /// </summary>
    /// <typeparam name="TDomainEvent">The type of domain event to handle</typeparam>
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Handles the domain event asynchronously.
        /// </summary>
        Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}