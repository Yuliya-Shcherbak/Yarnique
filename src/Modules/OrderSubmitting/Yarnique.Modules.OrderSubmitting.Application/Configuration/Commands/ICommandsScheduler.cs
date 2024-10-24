using Yarnique.Modules.OrderSubmitting.Application.Contracts;

namespace Yarnique.Modules.OrderSubmitting.Application.Configuration.Commands
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync(ICommand command);

        Task EnqueueAsync<T>(ICommand<T> command);
    }
}
