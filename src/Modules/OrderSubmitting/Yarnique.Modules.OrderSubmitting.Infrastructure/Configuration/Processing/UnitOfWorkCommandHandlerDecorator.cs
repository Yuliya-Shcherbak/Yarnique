using Microsoft.EntityFrameworkCore;
using Yarnique.Common.Infrastructure.UnitOfWork;
using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Common.Application.Contracts;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Processing
{
    internal class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
    {
        private readonly ICommandHandler<T> _decorated;
        private readonly IUnitOfWork _unitOfWork;
        private readonly OrderSubmittingContext _designsContext;

        public UnitOfWorkCommandHandlerDecorator(
            ICommandHandler<T> decorated,
            IUnitOfWork unitOfWork,
            OrderSubmittingContext designsContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _designsContext = designsContext;
        }

        public async Task Handle(T command, CancellationToken cancellationToken)
        {
            await this._decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase)
            {
                var internalCommand = await _designsContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await this._unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
