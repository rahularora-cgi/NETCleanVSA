namespace Framework.Domain
{
    public abstract class Entity<TId> : IEntityBase
    {
        public TId Id { get; protected set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime ModifiedAtUtc { get; set; }

    }
}
