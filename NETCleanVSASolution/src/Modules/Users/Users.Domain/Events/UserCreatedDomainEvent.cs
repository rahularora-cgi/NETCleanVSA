namespace Users.Domain.Events
{
    public record UserCreatedDomainEvent : DomainEventBase
    {
        int userId;
        string UserName;
        string Email;
    }

}
