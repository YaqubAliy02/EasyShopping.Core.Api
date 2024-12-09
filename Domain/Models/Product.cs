namespace Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public string Thumbnail { get; set; }
        public decimal SellingPrice { get; set; }
        public int StockQuantity { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid UserId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<ShoppingCart> ShoppingCart { get; set; } = new List<ShoppingCart>();
    }
}