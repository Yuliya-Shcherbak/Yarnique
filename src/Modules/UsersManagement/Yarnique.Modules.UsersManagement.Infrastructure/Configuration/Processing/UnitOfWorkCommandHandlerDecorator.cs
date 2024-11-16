using Yarnique.Common.Infrastructure.UnitOfWork;
using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Common.Application.Contracts;

namespace Yarnique.Modules.UsersManagement.Infrastructure.Configuration.Processing
{
    internal class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
    {
        private readonly ICommandHandler<T> _decorated;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsersManagementContext _usersManagementContext;

        public UnitOfWorkCommandHandlerDecorator(
            ICommandHandler<T> decorated,
            IUnitOfWork unitOfWork,
            UsersManagementContext usersManagementContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _usersManagementContext = usersManagementContext;
        }

        public async Task Handle(T command, CancellationToken cancellationToken)
        {
            await this._decorated.Handle(command, cancellationToken);

            await this._unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
