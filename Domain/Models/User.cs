namespace Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ShoppingCart> ShoppingCart { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<SubComment> SubComments { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
