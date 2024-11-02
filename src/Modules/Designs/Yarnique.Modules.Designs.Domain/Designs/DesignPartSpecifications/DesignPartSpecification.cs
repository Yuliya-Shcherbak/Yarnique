using Yarnique.Common.Domain;
using Yarnique.Modules.Designs.Domain.Designs.DesignParts;
using Yarnique.Modules.Designs.Domain.Designs.Events;

namespace Yarnique.Modules.Designs.Domain.Designs.DesignPartSpecifications
{
    public class DesignPartSpecification : Entity
    {
        public DesignPartSpecificationId Id { get; private set; }

        private DesignPartId _designPartId;

        private int _yarnAmount;
        private string _term;

        public DesignPartSpecification() { }

        public static DesignPartSpecification Create(DesignPartId designPartId, int yarnAmount, string term)
        {
            return new DesignPartSpecification(Guid.NewGuid(), designPartId, yarnAmount, term);
        }

        private DesignPartSpecification(Guid id, DesignPartId designPartId, int yarnAmount, string term)
        {
            Id = new DesignPartSpecificationId(id);
            _designPartId = designPartId;
            _yarnAmount = yarnAmount;
            _term = term;

            AddDomainEvent(new DesignPartSpecificationCreatedDomainEvent(Id));
        }
    }
}
