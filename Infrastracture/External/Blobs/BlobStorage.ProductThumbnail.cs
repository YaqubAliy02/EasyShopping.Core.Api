using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Models;

namespace Infrastracture.External.Blobs
{
    public partial class BlobStorage
    {
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
    }
}
