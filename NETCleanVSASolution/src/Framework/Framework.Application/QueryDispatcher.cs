namespace Framework.Application
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public async Task<Result<TResponse>> QueryAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery<TResponse>
        {
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
            return await handler.HandleAsync(query, cancellationToken);
        }

    }
}
