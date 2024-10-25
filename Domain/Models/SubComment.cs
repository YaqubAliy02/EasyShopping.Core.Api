namespace Domain.Models
{
    public class SubComment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
