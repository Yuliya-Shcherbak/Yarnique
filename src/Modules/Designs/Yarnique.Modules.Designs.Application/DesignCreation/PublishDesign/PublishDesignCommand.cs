using Yarnique.Modules.Designs.Application.Contracts;
using Yarnique.Modules.Designs.Domain.Designs.Designs;

namespace Yarnique.Modules.Designs.Application.DesignCreation.PublishDesign
{
    public class PublishDesignCommand : CommandBase
    {
        public PublishDesignCommand(Guid designId)
        {
            DesignId = new DesignId(designId);
        }

        public DesignId DesignId { get; }
    }
}
