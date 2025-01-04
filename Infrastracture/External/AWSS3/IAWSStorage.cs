using Domain.Models;

namespace Infrastracture.External.AWSS3
{
    public interface IAWSStorage
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task DeleteFileAsync(string fileName);
        Task<List<ProductThumbnail>> GetAllFilesAsync();
    }
}
