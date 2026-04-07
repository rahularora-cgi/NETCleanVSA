using Framework.Application;
using Framework.Infrastructure.Notifications;
using Microsoft.Extensions.Logging;
using Users.Domain.Events;

namespace Users.Application.Events
{
    /// <summary>
    /// Handles UserCreatedDomainEvent using the custom IDomainEventHandler pattern.
    /// Sends welcome email notifications to newly created users.
    /// Follows the same pattern as command and query handlers in the application.
    /// This handler is used when domain events are processed in-memory via IDomainEventDispatcher.
    /// </summary>
    public class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
    {
        private readonly INotificationService<EmailNotificationMessage> _emailNotificationService;
        private readonly ILogger<UserCreatedDomainEventHandler> _logger;

        public UserCreatedDomainEventHandler(
            INotificationService<EmailNotificationMessage> emailNotificationService,
            ILogger<UserCreatedDomainEventHandler> logger)
        {
            _emailNotificationService = emailNotificationService;
            _logger = logger;
        }

        public async Task HandleAsync(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Handling UserCreatedDomainEvent for user {UserId} with email {Email}",
                domainEvent.User.Id,
                domainEvent.User.Email);

            try
            {
                await SendWelcomeEmail(domainEvent, cancellationToken);

                _logger.LogInformation(
                    "Successfully sent welcome email to user {UserId} at {Email}",
                    domainEvent.User.Id,
                    domainEvent.User.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send welcome email to user {UserId} at {Email}",
                    domainEvent.User.Id,
                    domainEvent.User.Email);
                
                // Re-throw to ensure the transaction is rolled back
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