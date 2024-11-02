namespace Yarnique.Modules.OrderSubmitting.Domain.Orders
{
    public interface IOrderSubmittingRepository
    {
        Task AddAsync(Order order);

        Task<Order> GetByIdAsync(OrderId id);
    }
}
