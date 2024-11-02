using Microsoft.EntityFrameworkCore;
using Yarnique.Modules.OrderSubmitting.Domain.Orders;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Domain
{
    public class OrderSubmittingRepository : IOrderSubmittingRepository
    {
        private readonly OrderSubmittingContext _orderSubmittingContext;

        public OrderSubmittingRepository(OrderSubmittingContext orderSubmittingContext)
        {
            _orderSubmittingContext = orderSubmittingContext;
        }

        public async Task AddAsync(Order order)
        {
            await _orderSubmittingContext.Orders.AddAsync(order);
        }

        public async Task<Order> GetByIdAsync(OrderId orderId)
        {
            return await _orderSubmittingContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        }
    }
}
