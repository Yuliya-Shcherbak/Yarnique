using Yarnique.Common.Domain;
using Yarnique.Modules.Designs.Domain.Designs.Designs;

namespace Yarnique.Modules.Designs.Domain.Designs.Events
{
    public class DesignCreatedDomainEvent : DomainEventBase
    {
        public DesignCreatedDomainEvent(DesignId id)
        {
            Id = id;
        }

        public new DesignId Id { get; }
    }
}
