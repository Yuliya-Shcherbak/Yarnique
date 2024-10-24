using Yarnique.Common.Domain;
using Yarnique.Modules.Designs.Domain.Designs.Designs;

namespace Yarnique.Modules.Designs.Domain.Designs.Events
{
    public class DesignPublishedDomainEvent : DomainEventBase
    {
        public DesignPublishedDomainEvent(Guid designId)
        {
            DesignId = designId;
        }

        public Guid DesignId { get; }
    }
}
