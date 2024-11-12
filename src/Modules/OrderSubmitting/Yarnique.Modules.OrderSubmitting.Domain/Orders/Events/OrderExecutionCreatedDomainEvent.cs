using Yarnique.Common.Domain;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.OrderExecutions;

namespace Yarnique.Modules.OrderSubmitting.Domain.Orders.Events
{
    internal class OrderExecutionCreatedDomainEvent : DomainEventBase
    {
        public OrderExecutionCreatedDomainEvent(OrderExecutionId orderExecutionId)
        {
            OrderExecutionId = orderExecutionId;
        }

        public OrderExecutionId OrderExecutionId { get; }
    }
}
