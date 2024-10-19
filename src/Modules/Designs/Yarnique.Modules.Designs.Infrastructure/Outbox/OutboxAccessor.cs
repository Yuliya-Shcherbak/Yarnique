using Yarnique.Common.Application.Outbox;

namespace Yarnique.Modules.Designs.Infrastructure.Outbox
{
    public class OutboxAccessor : IOutbox
    {
        private readonly DesignsContext _designsContext;

        public OutboxAccessor(DesignsContext designsContext)
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
