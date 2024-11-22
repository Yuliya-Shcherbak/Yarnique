using Microsoft.EntityFrameworkCore;
using Yarnique.Modules.OrderSubmitting.Domain.Designs;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Domain
{
    public class DesignRepository : IDesignRepository
    {
        private readonly OrderSubmittingContext _orderSubmittingContext;

        public DesignRepository(OrderSubmittingContext orderSubmittingContext)
        {
            _orderSubmittingContext = orderSubmittingContext;
        }

        public async Task<List<DesignPartSpecification>> GetDesignPartSpecificationsByDesignIdAsync(DesignId designId)
        {
            return await _orderSubmittingContext.DesignPartSpecifications
                .Where(c => EF.Property<DesignId>(c, "_designId") == designId)
                .OrderBy(c => EF.Property<int>(c, "_executionOrder"))
                .ToListAsync();
        }
    }
}
