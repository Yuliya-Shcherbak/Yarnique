using MediatR;
using Yarnique.Common.Infrastructure.EventBus;
using Yarnique.Modules.Designs.IntegrationEvents;

namespace Yarnique.Modules.Designs.Application.DesignCreation.PublishDesign
{
    internal class DesignPublishedNotificationHandler : INotificationHandler<DesignPublishedNotification>
    {
        private readonly IEventsBus _eventBus;

        internal DesignPublishedNotificationHandler(IEventsBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task Handle(DesignPublishedNotification notification, CancellationToken cancellationToken)
        {
            await _eventBus.Publish(
                new DesignPublishedIntegrationEvent(
                    Guid.NewGuid(),
                    notification.DomainEvent.OccurredOn,
                    notification.DomainEvent.DesignId
                ));
        }
    }
}
