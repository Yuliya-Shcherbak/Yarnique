using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Modules.Designs.Domain.Designs;
using Yarnique.Modules.Designs.Domain.Designs.DesignParts;

namespace Yarnique.Modules.Designs.Application.DesignCreation.CreateDesignPart
{
    internal class CreateDesignPartCommandHandler : ICommandHandler<CreateDesignPartCommand, Guid>
    {
        private readonly IDesignsRepository _designsRepository;

        public CreateDesignPartCommandHandler(IDesignsRepository designsRepository)
        {
            _designsRepository = designsRepository;
        }

        public async Task<Guid> Handle(CreateDesignPartCommand command, CancellationToken cancellationToken)
        {
            var designPart = DesignPart.Create(command.Name);

            await _designsRepository.AddDesignPartAsync(designPart);

            return designPart.Id.Value;
        }
    }
}
