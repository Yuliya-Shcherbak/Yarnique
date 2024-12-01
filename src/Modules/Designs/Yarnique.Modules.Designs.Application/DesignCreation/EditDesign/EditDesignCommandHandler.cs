using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Modules.Designs.Domain.Designs;
using Yarnique.Modules.Designs.Domain.Designs.DesignPartSpecifications;

namespace Yarnique.Modules.Designs.Application.DesignCreation.EditDesign
{
    internal class EditDesignCommandHandler : ICommandHandler<EditDesignCommand>
    {
        private readonly IDesignsRepository _designsRepository;

        public EditDesignCommandHandler(IDesignsRepository designsRepository)
        {
            _designsRepository = designsRepository;
        }

        public async Task Handle(EditDesignCommand command, CancellationToken cancellationToken)
        {
            var design = await _designsRepository.GetDesignByIdAsync(command.DesignId);
            var parts = command.Parts.Select(x => DesignPartSpecification.Create(x.DesignPartId, x.YarnAmount, x.Order, x.Term)).ToList();
            design.Update(command.Name, command.Price, parts);
        }
    }
}
