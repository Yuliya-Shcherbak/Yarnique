using Dapper;
using Yarnique.Common.Application.Data;
using Yarnique.Modules.UsersManagement.Application.Authentication.PasswordManagement;
using Yarnique.Modules.UsersManagement.Application.Authentication.TokenManagement;
using Yarnique.Common.Application.Configuration.Commands;

namespace Yarnique.Modules.UsersManagement.Application.Authentication.Authenticate
{
    internal class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommand, AuthenticationResult>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly ITokenService _tokenService;

        internal AuthenticateCommandHandler(ISqlConnectionFactory sqlConnectionFactory, ITokenService tokenService)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _tokenService = tokenService;
        }

        public async Task<AuthenticationResult> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = $"""
                SELECT 
                    [User].[Id] as [{nameof(UserDto.Id)}],
                    [User].[UserName] as [{nameof(UserDto.UserName)}],
                    [User].[FirstName] as [{nameof(UserDto.FirstName)}],
                    [User].[LastName] as [{nameof(UserDto.LastName)}],
                    [User].[Email] as [{nameof(UserDto.Email)}],
                    [User].[IsActive] as [{nameof(UserDto.IsActive)}],
                    [User].[Role] as [{nameof(UserDto.Role)}],
                    [User].[Password] as [{nameof(UserDto.Password)}],
                    [User].[PasswordSalt]  as [{nameof(UserDto.PasswordSalt)}]
                FROM [users].[Users] AS [User] 
                WHERE [User].[UserName] = @UserName
                """;

            var user = await connection.QuerySingleAsync<UserDto>(sql, new { request.UserName });

            if (user == null || !PasswordHasher.IsPasswordMatch(request.Password, user.PasswordSalt, user.Password))
            {
                return new AuthenticationResult("Incorrect login or password");
            }

            if (!user.IsActive)
            {
                return new AuthenticationResult("User is not active");
            }

            var accessToken = _tokenService.CreateAccessToken(user);
            return new AuthenticationResult(null, accessToken);
        }
    }
}
