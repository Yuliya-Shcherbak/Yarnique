using Yarnique.Common.Domain;
using Yarnique.Modules.Designs.Domain.Designs.DesignParts;

namespace Yarnique.Modules.Designs.Domain.Designs.Events
{
    public class DesignPartCreatedDomainEvent : DomainEventBase
    {
        public DesignPartCreatedDomainEvent(DesignPartId id) 
        {
            Id = id;
        }

        public new DesignPartId Id { get; }
    }
}
