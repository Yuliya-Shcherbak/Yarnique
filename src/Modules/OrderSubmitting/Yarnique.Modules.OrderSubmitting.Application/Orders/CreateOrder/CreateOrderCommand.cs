using Yarnique.Common.Application.Contracts;
using Yarnique.Modules.OrderSubmitting.Domain.Designs;
using Yarnique.Modules.OrderSubmitting.Domain.Users;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.CreateOrder
{
    public class CreateOrderCommand : CommandBase<Guid>
    {
        public CreateOrderCommand(Guid userId, Guid designId, DateOnly executionDate)
        {
            UserId = new UserId(userId);
            DesignId = new DesignId(designId);
            ExecutionDate = executionDate;
        }

        public UserId UserId { get; }

        public DesignId DesignId { get; }

        public DateOnly ExecutionDate { get; }
    }
}
