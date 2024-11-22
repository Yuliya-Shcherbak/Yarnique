using Yarnique.Common.Domain;

namespace Yarnique.Modules.OrderSubmitting.Domain.Orders.OrderExecutions
{
    public class OrderExecutionId : TypedIdValueBase
    {
        public OrderExecutionId(Guid value) : base(value)
        {
        }
    }
}
