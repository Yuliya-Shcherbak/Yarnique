using Yarnique.Common.Domain;

namespace Yarnique.Modules.OrderSubmitting.Domain.Orders
{
    public class OrderId : TypedIdValueBase
    {
        public OrderId(Guid value) : base(value)
        {
        }
    }
}
