using Domain.Enums;

namespace Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public bool ReceivedStatus { get; set; } = false;
        public PaymentStatus PaymentStatus { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Payment Payment { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}

