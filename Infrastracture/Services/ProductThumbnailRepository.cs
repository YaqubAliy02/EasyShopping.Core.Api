using Application.Repository;
using Infrastracture.External.Blobs;

namespace Infrastracture.Services
{
    public class ProductThumbnailRepository : IProductThumbnailRepository
    {
        private readonly IBlobStorage blobStorage;

        public ProductThumbnailRepository(IBlobStorage blobStorage)
        {
            this.blobStorage = blobStorage;
        }

        public async Task<string> AddProductThumbnailAsync(Stream fileStream, string fileName, string contentType) =>
            await this.blobStorage.UploadProductThumbnailAsync(fileStream, fileName, contentType);
    }
}
