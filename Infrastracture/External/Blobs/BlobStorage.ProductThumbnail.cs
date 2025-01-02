using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Models;

namespace Infrastracture.External.Blobs
{
    public partial class BlobStorage
    {
        public async Task<string> UploadProductThumbnailAsync(Stream fileStream, string fileName, string contentType) =>
            await UploadAsync(fileStream, fileName, contentType);

        public async Task<bool> DeleteProductThumbnailAsync(string fileName, string containerName) =>
            await DeleteAsync(fileName, containerName);

        public async Task<List<ProductThumbnail>> SelectAllProductThumbnailAsync() =>
             await SelectAllAsync();
    }
}
