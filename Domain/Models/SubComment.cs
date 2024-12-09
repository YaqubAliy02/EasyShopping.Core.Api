namespace Domain.Models
{
    public class SubComment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid CommentId { get; set; }
    }
}
