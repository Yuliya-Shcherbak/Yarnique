using Autofac;
using Yarnique.Modules.UsersManagement.Application.Contracts;
using Yarnique.Modules.UsersManagement.Infrastructure;

namespace Yarnique.API.Modules.OrderSubmitting
{
    public class UsersManagementAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UsersManagementModule>()
                .As<IUsersManagementModule>()
                .InstancePerLifetimeScope();
        }
    }
}
