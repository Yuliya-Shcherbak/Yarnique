using MediatR;

namespace Yarnique.Modules.Consumables.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
