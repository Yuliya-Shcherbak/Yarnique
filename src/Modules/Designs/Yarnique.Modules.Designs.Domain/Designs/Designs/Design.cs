using Yarnique.Common.Domain;
using Yarnique.Modules.Designs.Domain.Designs.Events;
using Yarnique.Modules.Designs.Domain.Designs.Rules;
using Yarnique.Modules.Designs.Domain.Designs.DesignPartSpecifications;

namespace Yarnique.Modules.Designs.Domain.Designs.Designs
{
    public class Design : Entity
    {
        public DesignId Id { get; private set; }

        private string _name;
        private double _price;
        private bool _published;
        private List<DesignPartSpecification> _parts;

        private Design() 
        {
            _parts = [];
        }

        public static Design Create(string name, double price, List<DesignPartSpecification> parts)
        {
            return new Design(Guid.NewGuid(), name, price, parts);
        }

        public void Update(string name, double price, List<DesignPartSpecification> parts)
        {
            this._name = name;
            this._price = price;
            this._parts = parts;
        }

        public void Publish()
        {
            this._published = true;

            AddDomainEvent(new DesignPublishedDomainEvent(Id.Value));
        }

        private Design(Guid id, string name, double price, List<DesignPartSpecification> parts)
        {
            this.CheckRule(new DesignNameRequiredRule(name));

            Id = new DesignId(id);
            _name = name;
            _price = price;
            _parts = parts;
            _published = false;

            AddDomainEvent(new DesignCreatedDomainEvent(Id));
        }
    }
}
