using Yarnique.Modules.Designs.Application.Configuration.Commands;
using Yarnique.Modules.Designs.Domain.Designs;

namespace Yarnique.Modules.Designs.Application.DesignCreation.PublishDesign
{
    internal class PublishDesignCommandHandler : ICommandHandler<PublishDesignCommand>
    {
        private readonly IDesignsRepository _designsRepository;

        public PublishDesignCommandHandler(IDesignsRepository designsRepository)
        {
            _designsRepository = designsRepository;
        }

        public async Task Handle(PublishDesignCommand command, CancellationToken cancellationToken)
        {
            var design = await _designsRepository.GetByIdAsync(command.DesignId);
            design.Publish();
        }
    }
}
