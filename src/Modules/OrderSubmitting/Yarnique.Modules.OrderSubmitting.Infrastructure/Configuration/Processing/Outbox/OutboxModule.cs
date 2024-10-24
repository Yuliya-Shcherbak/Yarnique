using Autofac;
using Yarnique.Common.Application.Events;
using Yarnique.Common.Application.Outbox;
using Yarnique.Common.Infrastructure.DomainEventsDispatching;
using Yarnique.Common.Infrastructure;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Outbox;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Processing.Outbox
{
    internal class OutboxModule : Autofac.Module
    {
        private readonly BiDictionary<string, Type> _domainNotificationsMap;

        public OutboxModule(BiDictionary<string, Type> domainNotificationsMap)
        {
            _domainNotificationsMap = domainNotificationsMap;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OutboxAccessor>()
                .As<IOutbox>()
                .FindConstructorsWith(new AllConstructorFinder())
                .InstancePerLifetimeScope();

            this.CheckMappings();

            builder.RegisterType<DomainNotificationsMapper>()
                .As<IDomainNotificationsMapper>()
                .FindConstructorsWith(new AllConstructorFinder())
                .WithParameter("domainNotificationsMap", _domainNotificationsMap)
                .SingleInstance();
        }

        private void CheckMappings()
        {
            var domainEventNotifications = Assemblies.Application
                .GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(IDomainEventNotification)))
                .ToList();

            List<Type> notMappedNotifications = [];
            foreach (var domainEventNotification in domainEventNotifications)
            {
                _domainNotificationsMap.TryGetBySecond(domainEventNotification, out var name);

                if (name == null)
                {
                    notMappedNotifications.Add(domainEventNotification);
                }
            }

            if (notMappedNotifications.Any())
            {
                throw new ApplicationException($"Domain Event Notifications {notMappedNotifications.Select(x => x.FullName).Aggregate((x, y) => x + "," + y)} not mapped");
            }
        }
    }
}