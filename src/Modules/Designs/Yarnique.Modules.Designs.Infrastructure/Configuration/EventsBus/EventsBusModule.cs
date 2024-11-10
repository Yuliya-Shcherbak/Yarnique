using Autofac;
using Yarnique.Common.Infrastructure.EventBus;

namespace Yarnique.Modules.Designs.Infrastructure.Configuration.EventsBus
{
    internal class EventsBusModule : Autofac.Module
    {
        private readonly IEventsBus _eventsBus;

        public EventsBusModule(IEventsBus eventsBus)
        {
            _eventsBus = eventsBus;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_eventsBus).SingleInstance();
        }
    }
}
