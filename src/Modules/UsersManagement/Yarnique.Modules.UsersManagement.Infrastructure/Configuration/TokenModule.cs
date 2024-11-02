using Autofac;
using Yarnique.Modules.UsersManagement.Application.Authentication.TokenManagement;
using Yarnique.Modules.UsersManagement.Domain.Identity;

namespace Yarnique.Modules.UsersManagement.Infrastructure.Configuration
{
    internal class TokenModule : Autofac.Module
    {
        private readonly IdentityConfig _identityConfig;

        internal TokenModule(IdentityConfig identityConfig)
        {
            _identityConfig = identityConfig;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TokenService>()
                .As<ITokenService>()
                .WithParameter("identityConfig", _identityConfig)
                .SingleInstance();
        }
    }
}
