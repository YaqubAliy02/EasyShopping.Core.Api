using Application.Abstraction;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastracture.Data
{
    public class EasyShoppingDbContext : DbContext, IEasyShoppingDbContext
    {
        private readonly IConfiguration configuration;

        public EasyShoppingDbContext(DbContextOptions<EasyShoppingDbContext> options,
            IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(option => option.Email).IsUnique();
        }
    }
}
