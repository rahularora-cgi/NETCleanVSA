namespace Framework.Domain
{
    public abstract class Entity<TId> : IEntityBase
    {
        public TId Id { get; protected set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime ModifiedAtUtc { get; set; }

        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

        protected void AddDomainEvent(IDomainEvent domainEvent)
            => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents()
            => _domainEvents.Clear();

    }
}
