namespace Yarnique.Modules.UsersManagement.Domain.Identity
{
    public class IdentityConfig
    {
        public string JwtIssuer { get; set; }

        public string Secret { get; set; }

        public int TokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
