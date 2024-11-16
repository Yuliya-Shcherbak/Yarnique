using Yarnique.Common.Application.Contracts;

namespace Yarnique.Modules.UsersManagement.Application.Authentication.Authenticate
{
    public class AuthenticateCommand : CommandBase<AuthenticationResult>
    {
        public AuthenticateCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        public string UserName { get; }

        public string Password { get; }
    }
}
