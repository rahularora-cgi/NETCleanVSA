namespace Framework.Application
{
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
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
            return await handler.HandleAsync(command, cancellationToken);
        }

        //public async Task<Result> SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        //    where TCommand : ICommand  
        //{
        //    var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        //    return await handler.HandleAsync(command, cancellationToken);
        //}
      
    }

}
