namespace Application.DTOs.ProductThumbnails
{
    public class GetAllProductThumbnailDTO
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string BlobUri { get; set; }
        public Guid ProductId { get; set; }
        public DateTimeOffset UploadedDate { get; set; }
    }
}
