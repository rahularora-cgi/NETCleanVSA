namespace Framework.Application.Abstractions.Notification
{
    public interface INotificationService<TMessage> where TMessage : INotificationMessage
    {
        Task SendNotificationAsync(TMessage message, CancellationToken cancellationToken = default);
    }

    public interface INotificationMessage
    {
    }

    public record EmailNotificationMessage(string Recipient, string Subject, string Body) : INotificationMessage 
    {
    }

    public record TextNotificationMessage(string PhoneNumber, string Message) : INotificationMessage;

}
