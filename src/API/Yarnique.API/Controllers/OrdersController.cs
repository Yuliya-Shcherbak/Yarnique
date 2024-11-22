using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yarnique.API.Modules.OrderSubmitting.Orders;
using Yarnique.Modules.OrderSubmitting.Application.Contracts;
using Yarnique.Modules.OrderSubmitting.Application.Designs.GetDesigns;
using Yarnique.Modules.OrderSubmitting.Application.Orders.AcceptOrder;
using Yarnique.Modules.OrderSubmitting.Application.Orders.CreateOrder;
using Yarnique.Modules.OrderSubmitting.Application.Orders.PayOrder;

namespace Yarnique.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : BaseController
    {
        private readonly IOrderSubmittingModule _ordersModule;

        public OrdersController(IOrderSubmittingModule orderSubmittingModule)
        {
            _ordersModule = orderSubmittingModule;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var result = await _ordersModule.ExecuteCommandAsync(new CreateOrderCommand(_userId(), request.DesignId, request.ExecutionDate));

            return Ok(result);
        }

        [HttpPut("{orderId}/payment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PayOrder([FromRoute] Guid orderId, [FromBody] PaymentOrderRequest request)
        {
            await _ordersModule.ExecuteCommandAsync(new PayOrderCommand(_userId(), orderId, request.CardNumber, request.CardholderName));

            return Ok();
        }

        [HttpPut("{orderId}/accept")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AcceptOrder([FromRoute] Guid orderId)
        {
            await _ordersModule.ExecuteCommandAsync(new AcceptOrderCommand(orderId));

            return Ok();
        }

        [HttpGet("designs")]
        [ProducesResponseType(typeof(List<DesignDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDesigns()
        {
            var result = await _ordersModule.ExecuteQueryAsync(new GetDesignsQuery());

            return Ok(result);
        }
    }
}
