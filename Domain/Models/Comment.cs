namespace Domain.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
