using Microsoft.EntityFrameworkCore;
using Yarnique.Modules.OrderSubmitting.Domain.Orders;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.OrderExecutions;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Domain
{
    public class OrderSubmittingRepository : IOrderSubmittingRepository
    {
        private readonly OrderSubmittingContext _orderSubmittingContext;

        public OrderSubmittingRepository(OrderSubmittingContext orderSubmittingContext)
        {
            _orderSubmittingContext = orderSubmittingContext;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _orderSubmittingContext.Orders.AddAsync(order);
        }

        public async Task AddOrderExectutionAsync(OrderExecution orderExecution)
        {
            await _orderSubmittingContext.OrderExecutions.AddAsync(orderExecution);
        }

        public async Task<Order> GetOrderByIdAsync(OrderId orderId)
        {
            return await _orderSubmittingContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        }
    }
}
