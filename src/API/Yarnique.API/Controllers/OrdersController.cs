using Microsoft.AspNetCore.Mvc;
using Yarnique.Modules.OrderSubmitting.Application.Contracts;
using Yarnique.Modules.OrderSubmitting.Application.Designs.GetDesigns;

namespace Yarnique.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        private readonly IOrderSubmittingModule _ordersModule;

        public OrdersController(IOrderSubmittingModule orderSubmittingModule)
        {
            _ordersModule = orderSubmittingModule;
        }

        [HttpGet("designs")]
        [ProducesResponseType(typeof(List<DesignDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDesignById()
        {
            var result = await _ordersModule.ExecuteQueryAsync(new GetDesignsQuery());

            return Ok(result);
        }
    }
}
