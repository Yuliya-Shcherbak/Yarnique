using Yarnique.Modules.Designs.Application.Configuration.Commands;
using Yarnique.Modules.Designs.Domain.Designs;

namespace Yarnique.Modules.Designs.Application.DesignCreation.PublishDesign
{
    internal class PublishDesignComandHandler : ICommandHandler<PublishDesignComand>
    {
        private readonly IDesignsRepository _designsRepository;

        public PublishDesignComandHandler(IDesignsRepository designsRepository)
        {
            _designsRepository = designsRepository;
        }

        public async Task Handle(PublishDesignComand command, CancellationToken cancellationToken)
        {
            var design = await _designsRepository.GetByIdAsync(command.DesignId);
            design.Publish();

            //_designsRepository.PublishDesignAsync(design);
        }
    }
}
