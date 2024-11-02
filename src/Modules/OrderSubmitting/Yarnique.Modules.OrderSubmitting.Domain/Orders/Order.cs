using Yarnique.Common.Domain;
using Yarnique.Modules.OrderSubmitting.Domain.Designs;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Events;
using Yarnique.Modules.OrderSubmitting.Domain.Users;

namespace Yarnique.Modules.OrderSubmitting.Domain.Orders
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

        private Order()
        {
        }

        public static Order Create(UserId userId, DesignId designId, OrderStatus status, DateOnly executionDate)
        {
            return new Order(Guid.NewGuid(), userId, designId, status, executionDate);
        }

        public void AddPaymentInfo(bool isPaid, string transactionId, string transactionError)
        {
            this._isPaid = isPaid;
            this._transactionId = transactionId;
            this._transactionError = transactionError;
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
