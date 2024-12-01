using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Modules.Designs.Domain.Designs;

namespace Yarnique.Modules.Designs.Application.DesignCreation.UploadDesignPartPattern
{
    internal class UploadDesignPartPatternCommandHandler : ICommandHandler<UploadDesignPartPatternCommand>
    {
        private readonly IDesignsRepository _designsRepository;
        private readonly BlobServiceClient _blobServiceClient;

        public UploadDesignPartPatternCommandHandler(IDesignsRepository designsRepository, BlobServiceClient blobServiceClient)
        {
            _designsRepository = designsRepository;
            _blobServiceClient = blobServiceClient;
        }

        public async Task Handle(UploadDesignPartPatternCommand command, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("design-parts");
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            var designPart = await _designsRepository.GetDesignPartByIdAsync(command.DesignPartId);

            using var fileStream = command.File.OpenReadStream();

            var fileExtension = Path.GetExtension(command.File.FileName);
            var blobFileName = designPart.GetName().ToLowerInvariant().Replace(" ", "_") + fileExtension;

            var blobClient = containerClient.GetBlobClient(blobFileName);

            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = command.File.ContentType });

            designPart.UpdateBlobName(blobFileName);
        }
    }
}
