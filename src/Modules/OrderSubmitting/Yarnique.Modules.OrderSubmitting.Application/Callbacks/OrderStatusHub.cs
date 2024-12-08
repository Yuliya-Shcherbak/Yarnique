using Microsoft.AspNetCore.SignalR;

namespace Yarnique.Modules.OrderSubmitting.Application.Callbacks
{
    public class OrderStatusHub : Hub<IOrderStatusHub>
    {
        public override async Task OnConnectedAsync()
        {
            var context = Context.GetHttpContext();
            var orderId = Convert.ToString(Context.GetHttpContext().Request.Query["orderId"]);
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupNameByOrderId(orderId));
            await base.OnConnectedAsync();
        }

        public static string GetGroupNameByOrderId(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentNullException(nameof(orderId));
            }

            return $"orderId={orderId}";
        }
    }
}
