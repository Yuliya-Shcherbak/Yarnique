using Yarnique.Common.Application.Contracts;
using Yarnique.Modules.Designs.Domain.Users;

namespace Yarnique.Modules.Designs.Application.DesignCreation.CreateDesign
{
    public class CreateDesignCommand : CommandBase<Guid>
    {
        public CreateDesignCommand(string name, double price, Guid sellerId, List<CreateDesignPartSpecificationCommand> parts)
        {
            Name = name;
            Price = price;
            Parts = parts;
            SellerId = new UserId(sellerId);
        }

        public string Name { get; }
        public double Price { get; }
        public UserId SellerId {  get; }
        public List<CreateDesignPartSpecificationCommand> Parts { get; }
    }
}
