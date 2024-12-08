using Yarnique.Common.Domain;
using Yarnique.Common.Domain.OrderStatuses;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders;

namespace Yarnique.Modules.OrderSubmitting.Domain.Orders.Events
{
    public class OrderStatusChangedDomainEvent : DomainEventBase
    {
        public OrderStatusChangedDomainEvent(OrderId orderId, OrderStatus status)
        {
            OrderId = orderId;
            Status = status;
        }

        public OrderId OrderId { get; }
        public OrderStatus Status { get; }
    }
}
