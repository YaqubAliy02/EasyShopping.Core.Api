using Domain.Models;

namespace Infrastracture.External.Blobs
{
    public partial interface IBlobStorage
    {
        Task<string> UploadProductThumbnailAsync(Stream fileStream, string fileName, string contentType);
        Task<bool> DeleteProductThumbnailAsync(string fileName, string containerName);
        Task<List<ProductThumbnail>> SelectAllProductThumbnailAsync();
    }
}
