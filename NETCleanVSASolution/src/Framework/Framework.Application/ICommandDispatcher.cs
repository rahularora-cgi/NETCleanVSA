namespace Framework.Application
{

    public interface ICommandDispatcher
    {
        Task<Result<TResponse>> SendAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand<TResponse>;

        //Task<Result> SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        //    where TCommand : ICommand;

    }
}
