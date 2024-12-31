namespace Infrastracture.External.Blobs
{
    public partial interface IBlobStorage
    {
        Task<string> UploadProductThumbnailAsync(Stream fileStream, string fileName, string contentType);
        Task<IEnumerable<>>
    }
}
