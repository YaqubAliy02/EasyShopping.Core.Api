using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstraction
{
    public interface IEasyShoppingDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<UserRole> Roles { get; set; }
        DbSet<Payment> Payments { get; set; }
        DbSet<SubComment> SubComments { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<ShoppingCart> ShoppingCarts { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
