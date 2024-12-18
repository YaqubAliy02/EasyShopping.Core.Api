using Domain.Models;

namespace Application.DTOs.Comments
{
    public class GetCommentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid[] SubCommentsId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    }
}