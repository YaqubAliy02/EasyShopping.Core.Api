using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Models;

namespace Infrastracture.External.Blobs
{
    public partial class BlobStorage
    {
        private readonly BlobServiceClient blobServiceClient;

        public BlobStorage(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadProductThumbnailAsync(Stream fileStream, string fileName, string contentType) =>
            await UploadAsync(fileStream, fileName, contentType);

        public async Task<List<ProductThumbnail>> SelectAllProductThumbnailAsync()
        {
            var blobServiceClient = new BlobServiceClient(blobConnectionString);
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

        public async Task<bool> DeleteBlobAsync(string blobName, string containerName)
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
