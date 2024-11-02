using Yarnique.Modules.Designs.Application.Configuration.Commands;
using Yarnique.Modules.Designs.Domain.Designs;
using Yarnique.Modules.Designs.Domain.Designs.Designs;
using Yarnique.Modules.Designs.Domain.Designs.DesignPartSpecifications;

namespace Yarnique.Modules.Designs.Application.DesignCreation.CreateDesign
{
    internal class CreateDesignCommandHandler : ICommandHandler<CreateDesignCommand, Guid>
    {
        private readonly IDesignsRepository _designsRepository;

        public CreateDesignCommandHandler(IDesignsRepository designsRepository)
        {
            _designsRepository = designsRepository;
        }

        public async Task<Guid> Handle(CreateDesignCommand command, CancellationToken cancellationToken)
        {
            var parts = command.Parts.Select(x => DesignPartSpecification.Create(x.DesignPartId, x.YarnAmount, x.Term)).ToList();
            var design = Design.Create(command.Name, command.Price, parts);

            await _designsRepository.AddDesignAsync(design);

            return design.Id.Value;
        }
    }
}
