namespace Framework.Infrastructure.Notifications
{
    public class EmailNotificationService: INotificationService
    {
        public Task SendNotification()
        {
            // Implementation for sending email notification
            return Task.CompletedTask;
        }
    }
}
