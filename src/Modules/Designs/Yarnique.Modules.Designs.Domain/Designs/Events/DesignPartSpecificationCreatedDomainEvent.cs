using Yarnique.Common.Domain;
using Yarnique.Modules.Designs.Domain.Designs.DesignPartSpecifications;

namespace Yarnique.Modules.Designs.Domain.Designs.Events
{
    public class DesignPartSpecificationCreatedDomainEvent : DomainEventBase
    {
        public DesignPartSpecificationCreatedDomainEvent(DesignPartSpecificationId designPartSpecificationId)
        {
            DesignId = designPartSpecificationId;
        }

        public DesignPartSpecificationId DesignId { get; }
    }
}
