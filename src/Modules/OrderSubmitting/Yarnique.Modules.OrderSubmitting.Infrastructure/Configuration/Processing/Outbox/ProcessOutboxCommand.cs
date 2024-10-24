using Yarnique.Modules.OrderSubmitting.Application.Contracts;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Processing.Outbox
{
    public class ProcessOutboxCommand : CommandBase, IRecurringCommand
    {
    }
}
