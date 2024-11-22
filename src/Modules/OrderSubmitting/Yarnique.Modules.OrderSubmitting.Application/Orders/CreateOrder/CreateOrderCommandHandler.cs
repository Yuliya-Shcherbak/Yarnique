using Yarnique.Common.Domain.OrderStatuses;
using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Modules.OrderSubmitting.Domain.Orders;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.CreateOrder
{
    internal class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderSubmittingRepository _ordersRepository;

        public CreateOrderCommandHandler(IOrderSubmittingRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            // TODO : Add query of the DesignPartSpecification, calculate the approximate ExecutionDate and if requested date is earlier - set the Status as Negotiation
            // TODO : Query design and check if it is Discontinued -> add validation

            var order = Order.Create(command.UserId, command.DesignId, OrderStatus.Pending, command.ExecutionDate);

            await _ordersRepository.AddOrderAsync(order);
            return order.Id.Value;
        }
    }
}
