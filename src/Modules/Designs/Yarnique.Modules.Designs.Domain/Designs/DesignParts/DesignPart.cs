using Yarnique.Common.Domain;
using Yarnique.Modules.Designs.Domain.Designs.Events;
using Yarnique.Modules.Designs.Domain.Designs.Rules;

namespace Yarnique.Modules.Designs.Domain.Designs.DesignParts
{
    public class DesignPart : Entity
    {
        public DesignPartId Id { get; private set; }

        private string _name;

        private DesignPart() { }

        public static DesignPart Create(string name)
        {
            return new DesignPart(Guid.NewGuid(), name);
        }

        private DesignPart(Guid id, string name)
        {
            this.CheckRule(new DesignPartNameRequiredRule(name));

            Id = new DesignPartId(id);
            _name = name;

            AddDomainEvent(new DesignPartCreatedDomainEvent(Id));
        }
    }
}
