using Autofac;
using Yarnique.Common.Infrastructure.EventBus.Configuration;
using Yarnique.Common.Infrastructure.EventBus;
using Yarnique.Common.Infrastructure.EventBus.RabbitMqEventsBus;

namespace Yarnique.API.Modules.EventsBus
{
    internal class EventsBusAutofacModule : Autofac.Module
    {
        private readonly Serilog.ILogger _logger;
        private readonly RabbitMqConfig _rabbitMqConfig;

        public EventsBusAutofacModule(RabbitMqConfig rabbitMqConfig, Serilog.ILogger logger)
        {
            _rabbitMqConfig = rabbitMqConfig;
            _logger = logger;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var rabbitMqUri = $"amqp://{_rabbitMqConfig.UserName}:{_rabbitMqConfig.Password}@{_rabbitMqConfig.HostName}";
                return new RabbitMqEventBusClient(_logger, rabbitMqUri);
            })
            .As<IEventsBus>()
            .SingleInstance();
        }
    }
}
