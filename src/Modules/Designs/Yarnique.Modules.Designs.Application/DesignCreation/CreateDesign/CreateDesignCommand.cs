using Yarnique.Modules.Designs.Application.Contracts;

namespace Yarnique.Modules.Designs.Application.DesignCreation.CreateDesign
{
    public class CreateDesignCommand : CommandBase<Guid>
    {
        public CreateDesignCommand(string name, double price, List<CreateDesignPartSpecificationCommand> parts)
        {
            Name = name;
            Price = price;
            Parts = parts;
        }

        public string Name { get; }
        public double Price { get; }
        public List<CreateDesignPartSpecificationCommand> Parts { get; }
    }
}
