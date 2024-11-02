using Yarnique.Modules.Designs.Domain.Designs.DesignParts;

namespace Yarnique.Modules.Designs.Application.DesignCreation.CreateDesign
{
    public class CreateDesignPartSpecificationCommand
    {
        public CreateDesignPartSpecificationCommand(Guid designPartId, int yarnAmount, string term) 
        {
            DesignPartId = new DesignPartId(designPartId);
            YarnAmount = yarnAmount;
            Term = term;
        }

        public DesignPartId DesignPartId { get; }
        public int YarnAmount { get; }
        public string Term { get; }
    }
}
