namespace Framework.Domain
{
    public interface IEntityBase
    {
        public DateTime CreatedAtUtc { get; set; }
        public DateTime ModifiedAtUtc { get; set; }
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
