using Yarnique.Modules.OrderSubmitting.Domain.Orders.OrderExecutions;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders;

namespace Yarnique.Modules.OrderSubmitting.Domain.Orders
{
    public interface IOrderSubmittingRepository
    {
        Task AddOrderAsync(Order order);

        Task AddOrderExectutionAsync(OrderExecution orderExecution);

        Task<Order> GetOrderByIdAsync(OrderId id);
    }
}
