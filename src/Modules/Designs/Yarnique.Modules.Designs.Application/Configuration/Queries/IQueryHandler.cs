using MediatR;
using Yarnique.Modules.Designs.Application.Contracts;

namespace Yarnique.Modules.Designs.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}
