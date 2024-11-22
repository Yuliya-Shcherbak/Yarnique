using Yarnique.Common.Domain;

namespace Yarnique.Modules.UsersManagement.Domain.Users.Events
{
    public class UserCreatedDomainEvent : DomainEventBase
    {
        public UserCreatedDomainEvent(UserId userId)
        {
            UserId = userId;
        }

        public UserId UserId { get; }
    }
}
