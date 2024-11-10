using Autofac;
using Serilog;
using Yarnique.Common.Infrastructure.EventBus;

namespace Yarnique.Test.Common
{
    public class EventsBusModuleMock : Autofac.Module
    {
        private readonly ILogger _logger;

        public EventsBusModuleMock(ILogger logger)
        {
            _logger = logger;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                return new InMemoryEventBusClient(_logger);
            })
            .As<IEventsBus>()
            .SingleInstance();
        }
    }
}
