using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace Yarnique.Common.Infrastructure.EventBus.RabbitMqEventsBus
{
    public class RabbitMqEventBusClient : IEventsBus
    {
        private readonly ILogger _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const int MaxRetryAttempts = 3;
        private readonly TimeSpan InitialDelay = TimeSpan.FromSeconds(2);

        public RabbitMqEventBusClient(ILogger logger, string rabbitMqUri)
        {
            _logger = logger;

            var factory = new ConnectionFactory() { Uri = new Uri(rabbitMqUri) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "integration_events", type: ExchangeType.Headers);

            _channel.ExchangeDeclare(exchange: "dead_letter_exchange", type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: "dead_letter_queue", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: "dead_letter_queue", exchange: "dead_letter_exchange", routingKey: "");
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        public async Task Publish<T>(T @event) where T : IntegrationEvent
        {
            var eventName = typeof(T).Name;
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object>
            {
                { "event_type", eventName },
                { "content_type", "application/json" }
            };

            _channel.BasicPublish(
                exchange: "integration_events",
                routingKey: "",
                basicProperties: properties,
                body: body);

            _logger.Information("Published event {EventName} to RabbitMQ with headers", eventName);
        }

        public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
        {
            var eventName = typeof(T).Name;
            var queueName = $"{eventName}_queue";

            var headers = new Dictionary<string, object>
            {
                { "x-match", "all" },
                { "event_type", eventName }
            };

            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "dead_letter_exchange" },
                { "x-dead-letter-routing-key", "" }
            });

            _channel.QueueBind(queue: queueName, exchange: "integration_events", routingKey: "", arguments: headers);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var integrationEvent = JsonConvert.DeserializeObject<T>(message);

                var attempt = 0;
                var delay = InitialDelay;

                while (attempt < MaxRetryAttempts)
                {
                    try
                    {
                        await handler.Handle(integrationEvent);
                        _logger.Information("Handled event {EventName} from RabbitMQ with headers", eventName);
                        break;
                    }
                    catch (Exception ex)
                    {
                        attempt++;
                        _logger.Error(ex, "Error handling event {EventName}. Attempt {Attempt}/{MaxAttempts}", eventName, attempt, MaxRetryAttempts);

                        if (attempt >= MaxRetryAttempts)
                        {
                            _logger.Error("Failed to handle event {EventName} after {MaxAttempts} attempts. Routing to dead-message exchange.", eventName, MaxRetryAttempts);

                            
                            var dlxProperties = _channel.CreateBasicProperties();
                            dlxProperties.Headers = new Dictionary<string, object>
                            {
                                { "event_type", eventName },
                                { "x-failed-attempts", attempt },
                                { "content_type", "application/json" }
                            };

                            _channel.BasicPublish(
                                exchange: "dead_letter_exchange",
                                routingKey: "",
                                basicProperties: dlxProperties,
                                body: body);

                            break;
                        }

                        await Task.Delay(delay);
                        delay *= 2;
                    }
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            _logger.Information("Subscribed to event {EventName} on RabbitMQ with headers and dead-letter handling", eventName);
        }

        public void StartConsuming()
        { 
        }
    }
}
