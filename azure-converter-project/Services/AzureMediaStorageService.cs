using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using VideoConverter.Api.Utils;

namespace VideoConverter.Api.Services {
    public class AzureMediaStorageService : IMediaStorageService {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly CloudBlobClient _cloudBlobClient;
        private readonly CloudBlobContainer _cloudBlobContainer;
        private readonly AzureStorageAccountOptions _options;

        public AzureMediaStorageService (IOptions<AzureStorageAccountOptions> options) {
            _options = options.Value;
            if (!CloudStorageAccount.TryParse (_options.ConnectionString, out _cloudStorageAccount)) {
                throw new Exception ("Invalid connection string for Azure Storage Account");
            }
            _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient ();
            _cloudBlobContainer = _cloudBlobClient.GetContainerReference (_options.RootContainerName);
            if (_cloudBlobContainer.CreateIfNotExists ()) {
                BlobContainerPermissions permissions = new BlobContainerPermissions {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                _cloudBlobContainer.SetPermissions (permissions);
            }
        }

        public async Task<string> Upload (Stream stream, string name, string extension) {
            string blobName = $"{name ?? Guid.NewGuid ().ToString ()}.{extension ?? "blob"}";

            var cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference (blobName);

            if (extension != null) {
                if (extension.EndsWith("mp3"))
                cloudBlockBlob.Properties.ContentType = "audio/mpeg";
            }

            await cloudBlockBlob.UploadFromStreamAsync (stream);
            return cloudBlockBlob.Uri.ToString ();
        }
    }
}