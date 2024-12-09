using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}
