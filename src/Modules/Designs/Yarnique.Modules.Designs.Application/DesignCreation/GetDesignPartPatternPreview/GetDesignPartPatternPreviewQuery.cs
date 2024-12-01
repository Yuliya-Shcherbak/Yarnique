using Yarnique.Common.Application.Contracts;
using Yarnique.Modules.Designs.Domain.Designs.DesignParts;

namespace Yarnique.Modules.Designs.Application.DesignCreation.GetDesignPartPatternPreview
{
    public class GetDesignPartPatternPreviewQuery : QueryBase<Stream>
    {
        public GetDesignPartPatternPreviewQuery(Guid designPartId)
        {
            DesignPartId = new DesignPartId(designPartId);
        }

        public DesignPartId DesignPartId { get; }
    }
}
