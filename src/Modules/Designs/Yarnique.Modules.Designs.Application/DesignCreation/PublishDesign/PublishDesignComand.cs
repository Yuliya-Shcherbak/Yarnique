using Yarnique.Modules.Designs.Application.Contracts;
using Yarnique.Modules.Designs.Domain.Designs.Designs;

namespace Yarnique.Modules.Designs.Application.DesignCreation.PublishDesign
{
    public class PublishDesignComand : CommandBase
    {
        public PublishDesignComand(Guid designId)
        {
            DesignId = new DesignId(designId);
        }

        public DesignId DesignId { get; }
    }
}
