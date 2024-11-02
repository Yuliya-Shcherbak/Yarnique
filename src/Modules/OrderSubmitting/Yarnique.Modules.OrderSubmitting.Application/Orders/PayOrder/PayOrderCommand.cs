using Yarnique.Modules.OrderSubmitting.Application.Contracts;
using Yarnique.Modules.OrderSubmitting.Domain.Orders;
using Yarnique.Modules.OrderSubmitting.Domain.Users;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.PayOrder
{
    public class PayOrderCommand : CommandBase
    {
        public PayOrderCommand(Guid userId, Guid orderId, string cardNumber, string cardhorderName)
        {
            UserId = new UserId(userId);
            OrderId = new OrderId(orderId);
            CardNumber = cardNumber;
            CardholderName = cardhorderName;
        }

        public UserId UserId { get; }

        public OrderId OrderId { get; }

        public string CardholderName { get; }

        public string CardNumber { get; }
    }
}
