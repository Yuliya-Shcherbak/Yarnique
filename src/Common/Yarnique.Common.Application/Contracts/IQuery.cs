using MediatR;

namespace Yarnique.Common.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
