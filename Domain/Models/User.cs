
namespace Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Order> Orders { get; set; }  = new List<Order>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
