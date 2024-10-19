using Yarnique.Common.Infrastructure.EventBus;

namespace Yarnique.Modules.Designs.IntegrationEvents
{
    public class DesignPublishedIntegrationEvent : IntegrationEvent
    {
        public DesignPublishedIntegrationEvent(Guid id, DateTime occurredOn, Guid designId)
            : base(id, occurredOn)
        {
            DesignId = designId;
        }

        public Guid DesignId { get; }
    }
}
