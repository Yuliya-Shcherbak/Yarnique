﻿using Autofac;
using Autofac.Core;
using MediatR;
using Newtonsoft.Json;
using Yarnique.Common.Application.Events;
using Yarnique.Common.Application.Outbox;
using Yarnique.Common.Domain;
using Yarnique.Common.Infrastructure.Serialization;

namespace Yarnique.Common.Infrastructure.DomainEventsDispatching
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;

        private readonly ILifetimeScope _scope;

        private readonly IOutbox _outbox;

        private readonly IDomainEventsAccessor _domainEventsProvider;

        private readonly IDomainNotificationsMapper _domainNotificationsMapper;

        public DomainEventsDispatcher(
            IMediator mediator,
            ILifetimeScope scope,
            IOutbox outbox,
            IDomainEventsAccessor domainEventsProvider,
            IDomainNotificationsMapper domainNotificationsMapper)
        {
            _mediator = mediator;
            _scope = scope;
            _outbox = outbox;
            _domainEventsProvider = domainEventsProvider;
            _domainNotificationsMapper = domainNotificationsMapper;
        }

        public async Task DispatchEventsAsync()
        {
            var domainEvents = _domainEventsProvider.GetAllDomainEvents();

            List<IDomainEventNotification<IDomainEvent>> domainEventNotifications = [];
            foreach (var domainEvent in domainEvents)
            {
                Type domainEvenNotificationType = typeof(IDomainEventNotification<>);
                var domainNotificationWithGenericType = domainEvenNotificationType.MakeGenericType(domainEvent.GetType());
                var domainNotification = _scope.ResolveOptional(domainNotificationWithGenericType, new List<Parameter>
                {
                    new NamedParameter("domainEvent", domainEvent),
                    new NamedParameter("id", domainEvent.Id)
                });

                if (domainNotification != null)
                {
                    domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);
                }
            }

            _domainEventsProvider.ClearAllDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }

            foreach (var domainEventNotification in domainEventNotifications)
            {
                var type = _domainNotificationsMapper.GetName(domainEventNotification.GetType());
                var data = JsonConvert.SerializeObject(domainEventNotification, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                });

                var outboxMessage = new OutboxMessage(
                    domainEventNotification.Id,
                    domainEventNotification.DomainEvent.OccurredOn,
                    type,
                    data);

                _outbox.Add(outboxMessage);
            }
        }
    }
}
