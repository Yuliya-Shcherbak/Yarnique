using MediatR;

namespace Yarnique.Modules.Designs.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
