using Yarnique.Common.Domain;

namespace Yarnique.Modules.OrderSubmitting.Domain.Orders
{
    public class OrderStatus : ValueObject
    {
        public static OrderStatus Pendind => new OrderStatus(nameof(Pendind));

        public static OrderStatus Negotiation => new OrderStatus(nameof(Negotiation));

        public static OrderStatus Accepted => new OrderStatus(nameof(Accepted));

        public static OrderStatus InProgress => new OrderStatus(nameof(InProgress));

        public static OrderStatus OnHold => new OrderStatus(nameof(OnHold));

        public static OrderStatus Completed => new OrderStatus(nameof(Completed));

        public string Value { get; }

        private OrderStatus(string value)
        {
            this.Value = value;
        }
    }
}
