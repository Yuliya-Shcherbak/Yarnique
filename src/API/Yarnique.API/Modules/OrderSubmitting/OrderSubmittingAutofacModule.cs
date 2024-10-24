using Autofac;
using Yarnique.Modules.OrderSubmitting.Infrastructure;
using Yarnique.Modules.OrderSubmitting.Application.Contracts;

namespace Yarnique.API.Modules.OrderSubmitting
{
    public class OrderSubmittingAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OrderSubmittingModule>()
                .As<IOrderSubmittingModule>()
                .InstancePerLifetimeScope();
        }
    }
}
