using System.IO.Hashing;
using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IEasyShoppingDbContext easyShoppingDbContext;

        public CategoryRepository(IEasyShoppingDbContext easyShoppingDbContext)
        {
            this.easyShoppingDbContext = easyShoppingDbContext;
        }

        public async Task<Category> AddAsync(Category category)
        {
            await this.easyShoppingDbContext.Categories.AddAsync(category);
            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if (result > 0) return category;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await this.easyShoppingDbContext.Categories.FindAsync(id);

            if (category is null)
            {
                this.easyShoppingDbContext.Categories.Remove(category);
            }

            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if (result > 0) return true;

            return false;
        }

        public async Task<List<Category>> GetAsync(Expression<Func<Category, bool>> expression)
        {
            return await this.easyShoppingDbContext.Categories
                .Where(expression)
                .Include(category => category.Products)
                .ToListAsync();
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return this.easyShoppingDbContext.Categories
                .Where(c => c.Id.Equals(id))
                .Include(category => category.Products)
                .FirstOrDefault();
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var existingCategory = await GetByIdAsync(category.Id);

            if (existingCategory is not null)
            {
                existingCategory.Name = category.Name;

                foreach (var product in existingCategory.Products)
                {
                    var existingProducts = this.easyShoppingDbContext.Products.FindAsync(product.Id);

                    if (existingCategory is not null)
                    {
                        existingCategory.Products.Add(product);
                    }
                }

                var result = await this.easyShoppingDbContext.SaveChangesAsync();

                if (result > 0) return category;
            }

            return null;
        }
    }
}
