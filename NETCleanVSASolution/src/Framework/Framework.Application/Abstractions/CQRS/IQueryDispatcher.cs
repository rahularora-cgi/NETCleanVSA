namespace Framework.Application.Abstractions.CQRS
{
    public interface IQueryDispatcher
    {
        Task<Result<TResponse>> QueryAsync<TQuery, TResponse>(TQuery command, CancellationToken cancellationToken = default)
            where TQuery : IQuery<TResponse>;

        //Task<Result> QueryAsync<TQuery>(TQuery query, CancellationToken cancellationToken = default)
        //    where TQuery : IQuery<TResponse>;

    }
}
