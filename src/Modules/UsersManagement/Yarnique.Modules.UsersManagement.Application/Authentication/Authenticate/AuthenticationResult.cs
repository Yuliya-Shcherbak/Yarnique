namespace Yarnique.Modules.UsersManagement.Application.Authentication.Authenticate
{
    public class AuthenticationResult
    {
        public AuthenticationResult(string authenticationError, string token = null)
        {
            AuthenticationError = authenticationError;
            AccessToken = token;
        }

        public string AccessToken { get; }

        public string AuthenticationError { get; }
    }
}
