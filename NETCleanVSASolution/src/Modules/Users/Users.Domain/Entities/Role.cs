namespace Users.Domain.Entities
{
    public class Role<TKey>
    {
        public virtual TKey Id { get; set; }
        public virtual string RoleName { get; set; } = string.Empty;
    }

    public class Role: Role<Guid>
    {

    }

}