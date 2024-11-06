using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class UserRole
    {
        public Guid RoleId { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}


