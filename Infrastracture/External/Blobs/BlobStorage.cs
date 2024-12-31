using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace Infrastracture.External.Blobs
{
    public partial class BlobStorage : IBlobStorage
    {
        private readonly string blobConnectionString;
        private readonly string photoContainerName;

        private readonly HashSet<string> supportedPhotoExtensions =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".gif"
            };

        private readonly HashSet<string> supportedPhotoContentType =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "image/jpeg",
                "image/png",
                "image/gif"
            };


        public BlobStorage(IConfiguration configuration)
        {
            this.blobConnectionString = configuration["AzureBlobStorage:ConnectionString"];
            this.photoContainerName = configuration["AzureBlobStorage:PhotoContainerName"];
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            string containerName;

            if (supportedPhotoExtensions.Contains(extension) || supportedPhotoContentType.Contains(contentType))
            {
                containerName = photoContainerName;
            }
            else
            {
                throw new InvalidOperationException("Unsupported file extension or content type");
            }

            var blobServiceClient = new BlobServiceClient(blobConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType});

            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteAsync(string blobName, string containerName)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            try
            {
                if (!await containerClient.ExistsAsync())
                    throw new Exception($"Container {containerName} does not exist.");

                var blobClient = containerClient.GetBlobClient(blobName);

                if (!await blobClient.ExistsAsync())
                    throw new Exception($"Blob {blobName} does not exist.");

                await blobClient.DeleteAsync();

                return true;
            }
            catch (Exception exception)
            {
                return false;

                throw new Exception(exception.Message);
            }
        }
    }
}
