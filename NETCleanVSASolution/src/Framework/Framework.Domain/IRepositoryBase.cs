namespace Framework.Domain
{
    internal interface IRepositoryBase<T> where T : class
    {
        Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull;
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
