using Yarnique.Common.Domain;
using Yarnique.Common.Domain.OrderStatuses;
using Yarnique.Modules.OrderSubmitting.Domain.Designs;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Events;
using Yarnique.Modules.OrderSubmitting.Domain.Users;

namespace Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders
{
    public class Order : Entity
    {
        public OrderId Id { get; private set; }

        private UserId _userId;
        private DesignId _designId;
        private bool _isPaid;
        private string _transactionId;
        private string _transactionError;
        private OrderStatus _status;
        private DateOnly _executionDate;
        private DateTime? _acceptedDate;

        public DesignId DesignId { get; private set; }

        private Order(DesignId _designId)
        {
            DesignId = _designId;
        }

        public static Order Create(UserId userId, DesignId designId, OrderStatus status, DateOnly executionDate)
        {
            return new Order(Guid.NewGuid(), userId, designId, status, executionDate);
        }

        public void AddPaymentInfo(bool isPaid, string transactionId, string transactionError)
        {
            _isPaid = isPaid;
            _transactionId = transactionId;
            _transactionError = transactionError;
        }

        public void AcceptOrder()
        {
            _status = OrderStatus.Accepted;
            _acceptedDate = DateTime.UtcNow;

            AddDomainEvent(new OrderStatusChangedDomainEvent(Id, _status));
        }

        public void ChangeStatus(OrderStatus status)
        {
            _status = status;
            AddDomainEvent(new OrderStatusChangedDomainEvent(Id, _status));
        }

        private Order(Guid id, UserId userId, DesignId designId, OrderStatus status, DateOnly executionDate)
        {
            Id = new OrderId(id);

            _userId = userId;
            _designId = designId;
            _isPaid = false;
            _status = status;
            _executionDate = executionDate;

            AddDomainEvent(new OrderCreatedDomainEvent(Id));
        }
    }
}
