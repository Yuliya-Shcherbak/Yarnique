using Yarnique.Common.Domain;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders;

namespace Yarnique.Modules.OrderSubmitting.Domain.Orders.Events
{
    public class OrderCreatedDomainEvent : DomainEventBase
    {
        public OrderCreatedDomainEvent(OrderId orderId)
        {
            OrderId = orderId;
        }

        public OrderId OrderId { get; }
    }
}
