using Yarnique.Common.Application.Contracts;
using Yarnique.Common.Application.Pagination;

namespace Yarnique.Modules.OrderSubmitting.Application.Designs.GetDesigns
{
    public class GetDesignsQuery : QueryBaseWithPaging<PaginatedResponse<DesignDto>>
    {
        public GetDesignsQuery(int pugeNumber = 1, int pageSize = 5)
            :base(pugeNumber, pageSize)
        {
        }
    }
}
