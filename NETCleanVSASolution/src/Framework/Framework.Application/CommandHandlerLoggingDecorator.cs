namespace Framework.Application
{
    //public class CommandHandlerLoggingDecorator<TCommand, TResponse> : ICommandHandlerV1<TCommand, TResponse> where TCommand : ICommand<TResponse>
    //{
    //    private readonly ICommandHandlerV1<TCommand, TResponse> _commandHandler;

    //    public CommandHandlerLoggingDecorator(ICommandHandlerV1<TCommand, TResponse> commandHandler)
    //    {
    //        _commandHandler = commandHandler;
    //    }
    //    public async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    //    {
    //        var commandName = typeof(TCommand).Name;

    //        Console.WriteLine($"[Logging] Handling command of type {commandName}");

    //        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

    //        try
    //        {
    //            var result = await _commandHandler.HandleAsync(command, cancellationToken);
    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"[Logging] Exception while handling command of type {commandName}: {ex.Message}");
    //            throw;
    //        }
    //        finally
    //        {
    //            stopwatch.Stop();

    //            Console.WriteLine($"[Logging] Finished handling command of type {commandName} in {stopwatch.ElapsedMilliseconds} ms");
    //        }

    //    }
    //}
}
