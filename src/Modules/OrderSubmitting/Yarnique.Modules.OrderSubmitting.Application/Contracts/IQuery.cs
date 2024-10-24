using MediatR;

namespace Yarnique.Modules.OrderSubmitting.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
