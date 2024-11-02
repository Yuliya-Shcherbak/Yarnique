using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Yarnique.API.Controllers
{
    public abstract class BaseController : Controller
    {
        protected Guid _userId()
        {
            Guid.TryParse(
                HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value
                , out Guid userId);

            if (userId == Guid.Empty) throw new UnauthorizedAccessException();
            return userId;
        }
    }
}
