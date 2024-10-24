using Yarnique.Common.Application.Outbox;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Outbox
{
    public class OutboxAccessor : IOutbox
    {
        private readonly OrderSubmittingContext _designsContext;

        public OutboxAccessor(OrderSubmittingContext designsContext)
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
