using Autofac;
using Serilog;
using Yarnique.Common.Infrastructure.EventBus;

namespace Yarnique.Modules.Designs.Infrastructure.Configuration.EventsBus
{
    internal static class EventsBusStartup
    {
        internal static void Initialize(
            ILogger logger)
        {
            SubscribeToIntegrationEvents(logger);
        }

        private static void SubscribeToIntegrationEvents(ILogger logger)
        {
            var eventBus = DesignsCompositionRoot.BeginLifetimeScope().Resolve<IEventsBus>();
        }

        private static void SubscribeToIntegrationEvent<T>(IEventsBus eventBus, ILogger logger)
            where T : IntegrationEvent
        {
            logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
            eventBus.Subscribe(
                new IntegrationEventGenericHandler<T>());
        }
    }
}
