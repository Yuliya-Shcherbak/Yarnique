using MediatR;
using Yarnique.Modules.UsersManagement.Application.Contracts;

namespace Yarnique.Modules.UsersManagement.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}
