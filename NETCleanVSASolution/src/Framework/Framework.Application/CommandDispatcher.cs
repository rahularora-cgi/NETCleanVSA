namespace Framework.Application
{
    /// <summary>
    /// Dispatches command objects to their corresponding handlers for asynchronous processing.
    /// </summary>
    /// <remarks>This class resolves command handlers using the provided dependency injection service
    /// provider. It is typically used to decouple command invocation from command handling logic in applications
    /// following the CQRS pattern. Instances of this class are thread-safe if the underlying service provider and
    /// handlers are thread-safe.</remarks>
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<Result<TResponse>> SendAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default) 
            where TCommand : ICommand<TResponse>  
        {
            // Resolve the appropriate command handler from the service provider
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();

            // Invoke the handler's asynchronous method to process the command and return the result
            return await handler.HandleAsync(command, cancellationToken);
        }
    
    }

}
