using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Modules.OrderSubmitting.Domain.Orders;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.ChangeOrderStatus
{
    internal class ChangeOrderStatusCommandHandler : ICommandHandler<ChangeOrderStatusCommand>
    {
        private readonly IOrderSubmittingRepository _ordersRepository;

        public ChangeOrderStatusCommandHandler(IOrderSubmittingRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task Handle(ChangeOrderStatusCommand command, CancellationToken cancellationToken)
        {
            var order = await _ordersRepository.GetOrderByIdAsync(command.OrderId);

            order.ChangeStatus(command.Status);
        }
    }
}
