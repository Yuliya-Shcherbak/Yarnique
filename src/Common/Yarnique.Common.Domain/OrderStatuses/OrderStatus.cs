namespace Yarnique.Common.Domain.OrderStatuses
{
    public class OrderStatus : ValueObject
    {
        public static OrderStatus Pending => new OrderStatus(nameof(Pending));

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

        public static OrderStatus GetOrderStatus(string status)
        {
            var value = status?.Trim();
            return value?.ToLowerInvariant() switch
            {
                "pending" => Pending,
                "negotiation" => Negotiation,
                "accepted" => Accepted,
                "inprogress" => InProgress,
                "onhold" => OnHold,
                "completed" => Completed,
                _ => throw new ArgumentException($"Invalid OrderStatus value: {value}")
            };
        }
    }
}
