using Yarnique.Common.Application.Contracts;
using Yarnique.Common.Domain.OrderStatuses;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.ChangeOrderStatus
{
    public class ChangeOrderStatusCommand : CommandBase
    {
        public ChangeOrderStatusCommand(Guid orderId, string status)
        {
            OrderId = new OrderId(orderId);
            Status = OrderStatus.GetOrderStatus(status);
        }

        public OrderId OrderId { get; }
        public OrderStatus Status { get; }
    }
}
