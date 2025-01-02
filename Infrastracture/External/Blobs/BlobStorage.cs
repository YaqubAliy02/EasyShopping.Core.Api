using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Infrastracture.External.Blobs
{
    public partial class BlobStorage : IBlobStorage
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly string photoContainerName;

        private readonly HashSet<string> supportedPhotoExtensions =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".gif"
            };

        private readonly HashSet<string> supportedPhotoContentType =
            new(StringComparer.OrdinalIgnoreCase)
            {
                "image/jpeg",
                "image/png",
                "image/gif"
            };

        public BlobStorage(IConfiguration configuration, BlobServiceClient blobServiceClient)
        {
            this.photoContainerName = configuration["AzureBlobStorage:PhotoContainerName"];
            this.blobServiceClient = blobServiceClient;
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

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });

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
                // Log exception if necessary
                return false;
            }
        }

        public async Task<List<ProductThumbnail>> SelectAllAsync()
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(photoContainerName);
            var blobItems = blobContainerClient.GetBlobsAsync();
            var allowedExtensions = new[] { ".jpeg", ".jpg", ".png", ".gif" };

            var photos = new List<ProductThumbnail>();

            await foreach (BlobItem item in blobItems)
            {
                var blobClient = blobContainerClient.GetBlobClient(item.Name);
                var extension = Path.GetExtension(item.Name);

                if (allowedExtensions.Contains(extension))
                {
                    var properties = await blobClient.GetPropertiesAsync();

                    photos.Add(new ProductThumbnail
                    {
                        Id = Guid.NewGuid(),
                        FileName = item.Name,
                        ContentType = properties.Value.ContentType,
                        Size = properties.Value.ContentLength,
                        BlobUri = blobClient.Uri.ToString(),
                        UploadedDate = properties.Value.CreatedOn.DateTime,
                    });
                }
            }
            return photos;
        }
    }
}
