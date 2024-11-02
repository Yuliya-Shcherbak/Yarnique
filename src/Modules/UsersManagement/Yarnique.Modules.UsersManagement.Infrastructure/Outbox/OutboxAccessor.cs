using Yarnique.Common.Application.Outbox;

namespace Yarnique.Modules.UsersManagement.Infrastructure.Outbox
{
    public class OutboxAccessor : IOutbox
    {
        private readonly UsersManagementContext _designsContext;

        public OutboxAccessor(UsersManagementContext designsContext)
        {
            _designsContext = designsContext;
        }

        public void Add(OutboxMessage message)
        {
            _designsContext.OutboxMessages.Add(message);
        }

        public Task Save()
        {
            return Task.CompletedTask;
        }
    }
}
