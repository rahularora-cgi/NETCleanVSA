using Users.Domain.Entities;

namespace Users.Domain.Events
{
    public record UserCreatedDomainEvent : DomainEventBase
    {
        public UserCreatedDomainEvent(User user) => User = user;

        public User User { get; }
    }

}
