using Yarnique.Modules.Designs.Application.Contracts;

namespace Yarnique.Modules.Designs.Application.DesignCreation.GetDesign
{
    public class GetDesignQuery: QueryBase<DesignDto>
    {
        public GetDesignQuery(Guid designId)
        {
            DesignId = designId;
        }

        public Guid DesignId { get; }
    }
}
