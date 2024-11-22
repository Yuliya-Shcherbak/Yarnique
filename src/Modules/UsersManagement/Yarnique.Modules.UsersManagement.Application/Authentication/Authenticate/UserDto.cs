using System.Security.Claims;

namespace Yarnique.Modules.UsersManagement.Application.Authentication.Authenticate
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }
    }
}
