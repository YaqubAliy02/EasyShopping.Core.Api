namespace Domain.Models
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ProductId { get; set; }
        public Product Products { get; set; }
    }
}