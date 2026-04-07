using Framework.Infrastructure.Notifications;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Users.Domain.Events;

namespace Users.Application.Events
{
    /// <summary>
    /// Processes UserCreatedDomainEvent messages from the outbox and sends email notifications.
    /// This consumer is designed to work with the Outbox pattern implementation.
    /// </summary>
    public class UserCreatedEventConsumer
    {
        private readonly INotificationService<EmailNotificationMessage> _emailNotificationService;
        private readonly ILogger<UserCreatedEventConsumer> _logger;

        public UserCreatedEventConsumer(
            INotificationService<EmailNotificationMessage> emailNotificationService,
            ILogger<UserCreatedEventConsumer> logger)
        {
            _emailNotificationService = emailNotificationService;
            _logger = logger;
        }

        public async Task ConsumeAsync(string eventContent, CancellationToken cancellationToken = default)
        {
            try
            {
                // Deserialize the event from JSON
                var domainEvent = JsonSerializer.Deserialize<UserCreatedDomainEvent>(eventContent);
                
                if (domainEvent == null)
                {
                    _logger.LogError("Failed to deserialize UserCreatedDomainEvent from content: {Content}", eventContent);
                    return;
                }

                _logger.LogInformation(
                    "Processing UserCreatedDomainEvent for user {UserId} with email {Email}",
                    domainEvent.User.Id,
                    domainEvent.User.Email);

                await SendWelcomeEmail(domainEvent, cancellationToken);

                _logger.LogInformation(
                    "Successfully processed UserCreatedDomainEvent and sent welcome email to {Email}",
                    domainEvent.User.Email);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing UserCreatedDomainEvent: {Content}", eventContent);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing UserCreatedDomainEvent");
                throw;
            }
        }

        private async Task SendWelcomeEmail(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var user = domainEvent.User;

            _logger.LogInformation(
                "Sending welcome email to {FirstName} {LastName} at {Email}",
                user.FirstName,
                user.LastName,
                user.Email);

            var emailSubject = "Welcome to our platform!";
            var emailBody = $@"Dear {user.FirstName} {user.LastName},

Welcome to our platform! Your account has been successfully created.

Username: {user.UserName}
Email: {user.Email}

Please keep your credentials safe and secure.

Best regards,
The Team";

            await _emailNotificationService.SendNotificationAsync(
                new EmailNotificationMessage(user.Email, emailSubject, emailBody),
                cancellationToken);

            _logger.LogDebug(
                "Welcome email sent successfully for user {UserId}",
                user.Id);
        }
    }
}
