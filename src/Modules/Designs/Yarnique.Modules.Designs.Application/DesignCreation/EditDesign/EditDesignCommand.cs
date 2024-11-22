using Yarnique.Common.Application.Configuration.Attributes;
using Yarnique.Common.Application.Contracts;
using Yarnique.Modules.Designs.Application.DesignCreation.CreateDesign;
using Yarnique.Modules.Designs.Domain.Designs.Designs;

namespace Yarnique.Modules.Designs.Application.DesignCreation.EditDesign
{
    [CacheableEntity("DesignPart")]
    public class EditDesignCommand : CommandBase
    {
        public EditDesignCommand(Guid id, string name, double price, List<CreateDesignPartSpecificationCommand> parts)
        {
            DesignId = new DesignId(id);
            Name = name;
            Price = price;
            Parts = parts;
        }

        public DesignId DesignId { get; }
        public string Name { get; }
        public double Price { get; }
        public List<CreateDesignPartSpecificationCommand> Parts { get; }
    }
}
