using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly IEasyShoppingDbContext easyShoppingDbContext;

        public ProductRepository(IEasyShoppingDbContext easyShoppingDbContext)
        {
            this.easyShoppingDbContext = easyShoppingDbContext;
        }

        public async Task<Product> AddAsync(Product product)
        {
           await this.easyShoppingDbContext.Products.AddAsync(product);
            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if(result > 0) return product;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
           var product = await this.easyShoppingDbContext.Products.FindAsync(id);

            if(product is not null)
                this.easyShoppingDbContext.Products.Remove(product);

            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if(result > 0) return true;

            return false;
        }


        public async Task<IQueryable<Product>> GetAsync(Expression<Func<Product, bool>> expression)
        {
            return this.easyShoppingDbContext.Products
                 .Where(expression)
                 .Include(p => p.Category)
                 .Include(p => p.Comments)
                 .Include(p => p.ShoppingCart)
                 .Include(p => p.OrderItems);
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await this.easyShoppingDbContext.Products
           .Where(p => p.Id.Equals(id))
           .Include(p => p.Category)
           .Include(p => p.Comments)
           .Include(p => p.ShoppingCart)
           .Include(p => p.OrderItems)
           .FirstOrDefaultAsync();
        }

        public async Task<Product> UpdateAsync(Product updateProduct)
        {
            this.easyShoppingDbContext.Products.Update(updateProduct);

            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if (result > 0) return updateProduct;

            return null;
        }
    }
}
