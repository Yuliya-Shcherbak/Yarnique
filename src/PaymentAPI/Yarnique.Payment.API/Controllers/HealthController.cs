using Microsoft.AspNetCore.Mvc;

namespace Yarnique.Payment.API.Controllers
{
    [Route("health")]
    public class HealthController : Controller
    {
        public IActionResult Index()
        {
            return Ok(new { running = true });
        }
    }
}
