using Azure.Storage.Blobs;
using Dapper;
using FluentValidation;
using Yarnique.Common.Application.Configuration.Queries;
using Yarnique.Common.Application.Data;

namespace Yarnique.Modules.Designs.Application.DesignCreation.GetDesignPartPatternPreview
{
    internal class GetDesignPartPatternPreviewQueryHandler : IQueryHandler<GetDesignPartPatternPreviewQuery, Stream>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly BlobServiceClient _blobServiceClient;

        public GetDesignPartPatternPreviewQueryHandler(ISqlConnectionFactory sqlConnectionFactory, BlobServiceClient blobServiceClient)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<Stream> Handle(GetDesignPartPatternPreviewQuery query, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            var designPart = await connection.QuerySingleAsync<DesignPartBlobNameDto>("SELECT BlobName FROM [designs].[DesignParts] WHERE Id = @Id", new { Id = query.DesignPartId.Value });

            var validator = new DesignPartPatternUploadedValidator();
            validator.ValidateAndThrow(designPart);

            var containerClient = _blobServiceClient.GetBlobContainerClient("design-part-preview");
            var previewBlobName = designPart.BlobName.Split('.').First() + ".jpeg";
            var blobClient = containerClient.GetBlobClient(previewBlobName);

            MemoryStream stream = new MemoryStream();
            using var blobStream = await blobClient.OpenReadAsync();

            await blobStream.CopyToAsync(stream);
            stream.Position = 0;

            return stream;
        }
    }
}
