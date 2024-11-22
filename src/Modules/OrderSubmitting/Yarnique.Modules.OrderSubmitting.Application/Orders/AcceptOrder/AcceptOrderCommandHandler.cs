using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Modules.OrderSubmitting.Domain.Designs;
using Yarnique.Modules.OrderSubmitting.Domain.Orders;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.OrderExecutions;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.AcceptOrder
{
    internal class AcceptOrderCommandHandler : ICommandHandler<AcceptOrderCommand>
    {
        private readonly IOrderSubmittingRepository _ordersRepository;
        private readonly IDesignRepository _designsRepository;

        public AcceptOrderCommandHandler(IOrderSubmittingRepository ordersRepository, IDesignRepository designsRepository)
        {
            _ordersRepository = ordersRepository;
            _designsRepository = designsRepository;
        }

        public async Task Handle(AcceptOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _ordersRepository.GetOrderByIdAsync(command.OrderId);

            order.AcceptOrder();

            var designPartSpecifications = await _designsRepository.GetDesignPartSpecificationsByDesignIdAsync(order.DesignId);
            var dueDate = DateTime.UtcNow;
            foreach (var designPart in designPartSpecifications)
            {
                dueDate = dueDate.Add(TimeSpan.Parse(designPart.Term));
                var orderExecution = OrderExecution.Create(order.Id, designPart.Id, dueDate);
                await _ordersRepository.AddOrderExectutionAsync(orderExecution);
            }
        }
    }
}
