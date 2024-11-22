using Yarnique.Common.Domain;
using Yarnique.Common.Domain.OrderStatuses;
using Yarnique.Modules.OrderSubmitting.Domain.Designs;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Events;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders;

namespace Yarnique.Modules.OrderSubmitting.Domain.Orders.OrderExecutions
{
    public class OrderExecution : Entity
    {
        public OrderExecutionId Id { get; private set; }

        private OrderId _orderId;
        private DesignPartSpecificationId _designPartSpecificationId;
        private DateTime _dueDate;
        private ExecutionStatus _status;

        private OrderExecution()
        {
        }

        public static OrderExecution Create(OrderId orderId, DesignPartSpecificationId designPartSpecificationId, DateTime dueDate)
        {
            return new OrderExecution(Guid.NewGuid(), orderId, designPartSpecificationId, dueDate, ExecutionStatus.Pending);
        }

        public void UpdateStatus(ExecutionStatus status)
        {
            _status = status;
        }

        private OrderExecution(Guid id, OrderId orderId, DesignPartSpecificationId designPartSpecificationId, DateTime dueDate, ExecutionStatus status)
        {
            Id = new OrderExecutionId(id);

            _orderId = orderId;
            _designPartSpecificationId = designPartSpecificationId;
            _dueDate = dueDate;
            _status = status;

            AddDomainEvent(new OrderExecutionCreatedDomainEvent(Id));
        }
    }
}
