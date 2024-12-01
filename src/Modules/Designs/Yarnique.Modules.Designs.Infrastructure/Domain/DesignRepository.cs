using Microsoft.EntityFrameworkCore;
using Yarnique.Modules.Designs.Domain.Designs;
using Yarnique.Modules.Designs.Domain.Designs.Designs;
using Yarnique.Modules.Designs.Domain.Designs.DesignParts;

namespace Yarnique.Modules.Designs.Infrastructure.Domain
{
    public class DesignRepository : IDesignsRepository
    {
        private readonly DesignsContext _designsContext;

        public DesignRepository(DesignsContext designsContext)
        {
            _designsContext = designsContext;
        }
        public async Task AddDesignAsync(Design design)
        {
            await _designsContext.Designs.AddAsync(design);
        }

        public async Task AddDesignPartAsync(DesignPart designPart)
        {
            await _designsContext.DesignParts.AddAsync(designPart);
        }

        public void PublishDesignAsync(Design designPart)
        {
            _designsContext.Designs.Update(designPart);
        }

        public async Task<Design> GetDesignByIdAsync(DesignId id)
        {
            return await _designsContext.Designs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<DesignPart> GetDesignPartByIdAsync(DesignPartId id)
        {
            return await _designsContext.DesignParts.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
