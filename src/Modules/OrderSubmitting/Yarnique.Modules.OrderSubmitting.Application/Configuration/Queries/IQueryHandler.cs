using MediatR;
using Yarnique.Modules.OrderSubmitting.Application.Contracts;

namespace Yarnique.Modules.OrderSubmitting.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}
