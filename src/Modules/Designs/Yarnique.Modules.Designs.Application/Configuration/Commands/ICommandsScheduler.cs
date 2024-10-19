using Yarnique.Modules.Designs.Application.Contracts;

namespace Yarnique.Modules.Designs.Application.Configuration.Commands
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync(ICommand command);

        Task EnqueueAsync<T>(ICommand<T> command);
    }
}
