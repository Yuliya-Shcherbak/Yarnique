using Yarnique.Modules.UsersManagement.Application.Authentication.Authenticate;

namespace Yarnique.Modules.UsersManagement.Application.Authentication.TokenManagement
{
    public interface ITokenService
    {
        public string CreateAccessToken(UserDto user);
    }
}
