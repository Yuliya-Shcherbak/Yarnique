using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yarnique.API.Modules.UsersManagement.Authenticate;
using Yarnique.API.Modules.UsersManagement.Create;
using Yarnique.Modules.UsersManagement.Application.Authentication.Authenticate;
using Yarnique.Modules.UsersManagement.Application.Contracts;
using Yarnique.Modules.UsersManagement.Application.Users.CreateUser;

namespace Yarnique.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : BaseController
    {
        private readonly IUsersManagementModule _usersManagementModule;

        public UsersController(IUsersManagementModule usersManagementModule)
        {
            _usersManagementModule = usersManagementModule;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
        {
            var result = await _usersManagementModule.ExecuteCommandAsync(new AuthenticateCommand(request.UserName, request.Password));

            return Ok(result);
        }

        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await _usersManagementModule.ExecuteCommandAsync(new CreateUserCommand(request.UserName, request.FirstName, request.LastName, request.Email, request.Password, request.IsActive));

            return Ok(result);
        }
    }
}
