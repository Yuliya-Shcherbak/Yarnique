using Yarnique.Modules.Designs.Domain.Designs.DesignParts;
using Yarnique.Modules.Designs.Domain.Designs.Designs;

namespace Yarnique.Modules.Designs.Domain.Designs
{
    public interface IDesignsRepository
    {
        Task AddDesignAsync(Design designPart);

        Task AddDesignPartAsync(DesignPart designPart);

        void PublishDesignAsync(Design design);

        Task<Design> GetByIdAsync(DesignId id);
    }
}
