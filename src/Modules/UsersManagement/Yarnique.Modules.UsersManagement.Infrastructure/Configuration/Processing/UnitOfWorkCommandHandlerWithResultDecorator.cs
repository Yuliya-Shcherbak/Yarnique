using Yarnique.Common.Infrastructure.UnitOfWork;
using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Common.Application.Contracts;

namespace Yarnique.Modules.UsersManagement.Infrastructure.Configuration.Processing
{
    internal class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult>
        where T : ICommand<TResult>
    {
        private readonly ICommandHandler<T, TResult> _decorated;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsersManagementContext _usersManagementContext;

        public UnitOfWorkCommandHandlerWithResultDecorator(
            ICommandHandler<T, TResult> decorated,
            IUnitOfWork unitOfWork,
            UsersManagementContext usersManagementContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _usersManagementContext = usersManagementContext;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            var result = await this._decorated.Handle(command, cancellationToken);

            await this._unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
    }
}
