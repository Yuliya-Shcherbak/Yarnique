using Autofac;
using Azure.Storage.Blobs;

namespace Yarnique.API.Modules.BlobSorage
{
    public class BlobStorageAutofacModule : Autofac.Module
    {
        private readonly string _blobStorageConnectionString;

        public BlobStorageAutofacModule(string blobStorageConnectionString)
        {
            _blobStorageConnectionString = blobStorageConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                return new BlobServiceClient(_blobStorageConnectionString);
            })
            .As<BlobServiceClient>()
            .SingleInstance();
        }
    }
}
