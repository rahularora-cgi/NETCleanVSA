namespace Framework.Infrastructure.Messaging
{
    public class EventPublisher : IEventPublisher, IAsyncDisposable
    {
        private readonly MessageBrokerSettings _messageBrokerSettings;
        private readonly IConnection _rabbitMQconnection;
        private readonly IConnectionFactory _rabbitMQConnectionFactory;
        private readonly IChannel _rabbitMQChannel;

        public EventPublisher(IOptions<MessageBrokerSettings> _messageBrokerSettingsOptions)
        {
            _messageBrokerSettings = _messageBrokerSettingsOptions.Value;

            _rabbitMQConnectionFactory = new ConnectionFactory
            {
                HostName = _messageBrokerSettings.HostName,
                Port = _messageBrokerSettings.Port,
                UserName = _messageBrokerSettings.UserName,
                Password = _messageBrokerSettings.Password,
                VirtualHost = _messageBrokerSettings.VirtualHost
            };

            _rabbitMQconnection = _rabbitMQConnectionFactory.CreateConnectionAsync().Result;
            _rabbitMQChannel = _rabbitMQconnection.CreateChannelAsync().Result;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            var eventName = typeof(TEvent).Name;
            var exchangeName = "events";

            // Declare exchange (idempotent operation)
            await _rabbitMQChannel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            // Serialize event to JSON
            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            // Create message properties
            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                Headers = new Dictionary<string, object?>
                {
                    { "event-type", eventName }
                }
            };

            // Publish to exchange with routing key
            await _rabbitMQChannel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: eventName,
                mandatory: false,
                basicProperties: properties,
                body: body);
        }

        public async ValueTask DisposeAsync()
        {
            if (_rabbitMQChannel != null)
                await _rabbitMQChannel.CloseAsync();
            
            if (_rabbitMQconnection != null)
                await _rabbitMQconnection.CloseAsync();
        }
    }
}
