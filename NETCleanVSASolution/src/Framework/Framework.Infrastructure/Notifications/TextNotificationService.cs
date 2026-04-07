using Microsoft.Extensions.Logging;

namespace Framework.Infrastructure.Notifications
{
    public class TextNotificationService : INotificationService<TextNotificationMessage>
    {
        private readonly ILogger<TextNotificationService> _logger;

        public TextNotificationService(ILogger<TextNotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendNotificationAsync(TextNotificationMessage message, CancellationToken cancellationToken = default)
        {
            return SendTextAsync(message.PhoneNumber, message.Message, cancellationToken);
        }

        private async Task SendTextAsync(string phoneNumber, string message, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
             "Sending text message to {PhoneNumber} with message: {Message}",
             phoneNumber,
             message);

            throw new NotImplementedException();
        }
    }
}
