using Microsoft.EntityFrameworkCore;
using Yarnique.Common.Infrastructure.UnitOfWork;
using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Common.Application.Contracts;

namespace Yarnique.Modules.Designs.Infrastructure.Configuration.Processing
{
    internal class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult>
        where T : ICommand<TResult>
    {
        private readonly ICommandHandler<T, TResult> _decorated;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DesignsContext _designsContext;

        public UnitOfWorkCommandHandlerWithResultDecorator(
            ICommandHandler<T, TResult> decorated,
            IUnitOfWork unitOfWork,
            DesignsContext designsContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _designsContext = designsContext;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            var result = await this._decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase<TResult>)
            {
                var internalCommand = await _designsContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await this._unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
    }
}
