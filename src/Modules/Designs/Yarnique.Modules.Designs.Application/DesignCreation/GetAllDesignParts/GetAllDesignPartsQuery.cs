using Yarnique.Common.Application.Configuration.Attributes;
using Yarnique.Common.Application.Contracts;
using Yarnique.Common.Application.Pagination;

namespace Yarnique.Modules.Designs.Application.DesignCreation.GetAllDesignParts
{
    [CacheableEntity("DesignPart")]
    public class GetAllDesignPartsQuery : QueryBaseWithPaging<PaginatedResponse<DesignPartDto>>
    {
        public GetAllDesignPartsQuery(int pageNumber, int pageSize)
            : base(pageNumber, pageSize)
        {
        }
    }
}
