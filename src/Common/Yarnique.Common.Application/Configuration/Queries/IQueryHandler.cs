using MediatR;
using Yarnique.Common.Application.Contracts;

namespace Yarnique.Common.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}
