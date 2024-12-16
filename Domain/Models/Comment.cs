using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; } 
        public ICollection<SubComment> SubComments { get; set; } = new List<SubComment>();
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
