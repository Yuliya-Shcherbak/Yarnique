using Autofac;
using Serilog;
using Yarnique.Common.Infrastructure.EventBus;
using Yarnique.Modules.Designs.IntegrationEvents;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.EventsBus
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
            var eventBus = OrderSubmittingCompositionRoot.BeginLifetimeScope().Resolve<IEventsBus>();
            SubscribeToIntegrationEvent<DesignPublishedIntegrationEvent>(eventBus, logger);
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
