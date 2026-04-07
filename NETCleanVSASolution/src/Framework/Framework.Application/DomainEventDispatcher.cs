using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application
{
    /// <summary>
    /// Dispatcher implementation for publishing domain events to registered handlers.
    /// Follows the same pattern as CommandDispatcher and QueryDispatcher.
    /// </summary>
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default) 
            where TDomainEvent : IDomainEvent
        {
            // Get all registered handlers for this event type
            // Note: GetServices returns IEnumerable, allowing multiple handlers per event
            var handlers = _serviceProvider.GetServices<IDomainEventHandler<TDomainEvent>>();

            // Execute all handlers in parallel
            var tasks = handlers.Select(handler => handler.HandleAsync(domainEvent, cancellationToken));

            // Await all handler tasks to complete
            await Task.WhenAll(tasks);
        }
    }
}