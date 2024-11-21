using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Yarnique.Modules.UsersManagement.Application.Authentication.Authenticate;
using Yarnique.Modules.UsersManagement.Domain.Identity;

namespace Yarnique.Modules.UsersManagement.Application.Authentication.TokenManagement
{
    public class TokenService : ITokenService
    {
        private readonly IdentityConfig _identityConfig;

        public TokenService(IdentityConfig identityConfig)
        {
            _identityConfig = identityConfig;
        }

        public string CreateAccessToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_identityConfig.Secret);

            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            });

            var jwt = new SecurityTokenDescriptor
            {
                Issuer = _identityConfig.JwtIssuer,
                Subject = claims,
                Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_identityConfig.TokenExpiration)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(jwt);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
