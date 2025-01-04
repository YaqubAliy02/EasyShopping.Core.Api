namespace Domain.Models
{
    public class ProductThumbnail
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public string BlobUri { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public DateTimeOffset UploadedDate { get; set; }
    }
}
