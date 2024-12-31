namespace Application.Repository
{
    public interface IProductThumbnailRepository
    {
        Task<string> AddProductThumbnailAsync(Stream fileStream, string fileName, string contentType);

    }
}
