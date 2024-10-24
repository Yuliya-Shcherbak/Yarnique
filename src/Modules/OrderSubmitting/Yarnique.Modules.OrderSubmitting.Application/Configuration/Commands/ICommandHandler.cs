using MediatR;
using Yarnique.Modules.OrderSubmitting.Application.Contracts;

namespace Yarnique.Modules.OrderSubmitting.Application.Configuration.Commands
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
    {
    }

    public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
    }
}
