using Users.Domain.Events;

namespace Users.Domain.Entities
{
    public class User : AggregateRoot<Guid>
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAtUtc { get; set; }

        //Navigation property
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        //public ICollection<Claim> Claims { get; set; } = new List<Claim>();


        public static User Create(string userName, string email, string passwordHash, string firstName, string lastName)
        {
            var user = new User
            {
                UserName = userName,
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            user.AddDomainEvent(new UserCreatedDomainEvent(user));

            return user;
        }

    }
}
