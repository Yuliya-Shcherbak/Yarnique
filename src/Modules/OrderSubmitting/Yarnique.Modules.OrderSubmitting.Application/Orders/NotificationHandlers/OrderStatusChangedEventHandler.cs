using MediatR;
using Microsoft.AspNetCore.SignalR;
using Yarnique.Modules.OrderSubmitting.Application.Callbacks;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Events;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.NotificationHandlers
{
    internal class OrderStatusChangedEventHandler : INotificationHandler<OrderStatusChangedDomainEvent>
    {
        private readonly IHubContext<OrderStatusHub, IOrderStatusHub> _hubContext;

        public OrderStatusChangedEventHandler(IHubContext<OrderStatusHub, IOrderStatusHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task Handle(OrderStatusChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.Group(
                OrderStatusHub.GetGroupNameByOrderId(notification.OrderId.Value.ToString()))
                .OnOrderStatusChanged(notification.Status.Value);
        }
    }
}
