using Newtonsoft.Json;
using Yarnique.Common.Application.Events;
using Yarnique.Modules.Designs.Domain.Designs.Events;

namespace Yarnique.Modules.Designs.Application.DesignCreation.PublishDesign
{
    public class DesignPublishedNotification : DomainNotificationBase<DesignPublishedDomainEvent>
    {
        [JsonConstructor]
        public DesignPublishedNotification(DesignPublishedDomainEvent domainEvent, Guid id)
            : base(domainEvent, id)
        {
        }
    }
}
