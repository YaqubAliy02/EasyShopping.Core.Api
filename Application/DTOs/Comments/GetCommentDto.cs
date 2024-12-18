using Domain.Models;

namespace Application.DTOs.Comments
{
    public class GetCommentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public ICollection<SubComment> SubComments { get; set; } = new List<SubComment>();
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
