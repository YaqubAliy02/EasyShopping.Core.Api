namespace Domain.Models
{
    public class UserRole
    {
        public Guid RoleId { get; set; }
        public string Role { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}

