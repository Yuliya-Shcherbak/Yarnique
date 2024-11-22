namespace Yarnique.API.Modules.OrderSubmitting.Orders
{
    public class CreateOrderRequest
    {
        public Guid DesignId { get; set; }
        public DateOnly ExecutionDate { get; set; }
    }
}
