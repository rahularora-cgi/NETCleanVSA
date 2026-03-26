namespace Framework.Application
{
    public interface IQueryDispatcher
    {
        Task<Result<TResponse>> QueryAsync<TQuery, TResponse>(TQuery command, CancellationToken cancellationToken = default)
            where TQuery : IQuery<TResponse>;

        //Task<Result> QueryAsync<TQuery>(TQuery query, CancellationToken cancellationToken = default)
        //    where TQuery : IQuery<TResponse>;

    }
}
