using Autofac;
using Microsoft.AspNetCore.SignalR;
using Yarnique.Modules.OrderSubmitting.Application.Callbacks;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration
{
    internal class CallbackModule : Autofac.Module
    {
        private readonly IHubContext<OrderStatusHub, IOrderStatusHub> _hubContext;

        public CallbackModule(IHubContext<OrderStatusHub, IOrderStatusHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_hubContext);
        }
    }
}
