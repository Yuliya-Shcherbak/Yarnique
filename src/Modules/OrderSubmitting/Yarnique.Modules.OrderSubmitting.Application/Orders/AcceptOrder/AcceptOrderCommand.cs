using Yarnique.Common.Application.Contracts;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.AcceptOrder
{
    public class AcceptOrderCommand : CommandBase
    {
        public AcceptOrderCommand(Guid orderId) 
        { 
            OrderId = new OrderId(orderId);
        }

        public OrderId OrderId { get; set; }
    }
}
