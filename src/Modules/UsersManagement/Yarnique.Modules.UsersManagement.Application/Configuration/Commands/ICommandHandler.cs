using MediatR;
using Yarnique.Modules.UsersManagement.Application.Contracts;

namespace Yarnique.Modules.UsersManagement.Application.Configuration.Commands
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
    {
    }

    public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
    }
}
