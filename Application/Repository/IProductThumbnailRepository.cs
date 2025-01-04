using Domain.Models;

namespace Application.Repository
{
    public interface IProductThumbnailRepository : IRepository<ProductThumbnail>
    {
        Task<ProductThumbnail> AddProductThumbnailAsync(Guid productId, Stream photoStream, string fileName, string contentType);
        Task<ProductThumbnail> AddForUpdateProductThumbnailAsync(Stream photoStream, string fileName, string contentType);
    }
}
