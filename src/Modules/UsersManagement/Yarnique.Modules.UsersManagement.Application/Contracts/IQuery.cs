using MediatR;

namespace Yarnique.Modules.UsersManagement.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
