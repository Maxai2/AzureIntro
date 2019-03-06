using System;
using System.IO;
using System.Threading.Tasks;
using ConverterApp.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ConverterApp.Services {
    public class BlobService : IBlobService {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly CloudBlobClient _cloudBlobClient;
        private readonly CloudBlobContainer _cloudBlobContainer;

        public BlobService (string connectionString, string rootContainerName) {
            if (!CloudStorageAccount.TryParse (connectionString, out _cloudStorageAccount)) {
                throw new Exception ("Invalid connection string for Azure Storage Account");
            }
            _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient ();
            _cloudBlobContainer = _cloudBlobClient.GetContainerReference (rootContainerName);
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
                if (extension.EndsWith ("mp3"))
                    cloudBlockBlob.Properties.ContentType = "audio/mpeg";
            }

            await cloudBlockBlob.UploadFromStreamAsync (stream);
            return cloudBlockBlob.Uri.ToString ();
        }
    }
}