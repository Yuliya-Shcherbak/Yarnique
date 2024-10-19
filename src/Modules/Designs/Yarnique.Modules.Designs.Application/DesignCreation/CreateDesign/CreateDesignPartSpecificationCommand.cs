using Yarnique.Modules.Designs.Domain.Designs.DesignParts;

namespace Yarnique.Modules.Designs.Application.DesignCreation.CreateDesign
{
    public class CreateDesignPartSpecificationCommand
    {
        public CreateDesignPartSpecificationCommand(Guid designPartId, int yarnAmount) 
        {
            DesignPartId = new DesignPartId(designPartId);
            YarnAmount = yarnAmount;
        }

        public DesignPartId DesignPartId { get; }
        public int YarnAmount { get; }
    }
}
