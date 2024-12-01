using Autofac;
using Azure.Storage.Blobs;

namespace Yarnique.Modules.Designs.Infrastructure.Configuration
{
    internal class BlobServiceModule : Autofac.Module
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobServiceModule(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_blobServiceClient).SingleInstance();
        }
    }
}
