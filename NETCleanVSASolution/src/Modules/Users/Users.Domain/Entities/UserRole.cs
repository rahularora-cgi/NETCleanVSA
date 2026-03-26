namespace Users.Domain.Entities
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
