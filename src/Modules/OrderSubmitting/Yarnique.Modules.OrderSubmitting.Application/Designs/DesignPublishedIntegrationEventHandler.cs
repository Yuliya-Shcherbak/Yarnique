using MediatR;
using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Modules.Designs.IntegrationEvents;
using Yarnique.Modules.OrderSubmitting.Application.Designs.PublishDesign;

namespace Yarnique.Modules.OrderSubmitting.Application.Designs.Publish
{
    internal class DesignPublishedIntegrationEventHandler : INotificationHandler<DesignPublishedIntegrationEvent>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        internal DesignPublishedIntegrationEventHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(DesignPublishedIntegrationEvent _event, CancellationToken cancellationToken)
        {
            await _commandsScheduler.EnqueueAsync(new CopyPublishedDesignCommand(Guid.NewGuid(), _event.DesignId));
        }
    }
}
