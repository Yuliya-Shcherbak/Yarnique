using Yarnique.Common.Domain;
using Yarnique.Modules.Designs.Domain.Designs.Designs;

namespace Yarnique.Modules.Designs.Domain.Designs.Events
{
    public class DesignPublishedDomainEvent : DomainEventBase
    {
        public DesignPublishedDomainEvent(DesignId id)
        {
            DesignId = id;
        }

        public DesignId DesignId { get; }
    }
}
