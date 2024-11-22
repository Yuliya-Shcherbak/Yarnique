using Autofac;
using Yarnique.Modules.Designs.Application.Contracts;
using Yarnique.Modules.Designs.Infrastructure;

namespace Yarnique.API.Modules.Designs
{
    public class DesignsAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DesignsModule>()
                .As<IDesignsModule>()
                .InstancePerLifetimeScope();
        }
    }
}
