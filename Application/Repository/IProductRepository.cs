using Domain.Models;

namespace Application.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetAllProductsByIdAsync(IEnumerable<Guid> productId);
    }
}
