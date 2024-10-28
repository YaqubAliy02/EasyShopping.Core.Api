using System.Linq.Expressions;

namespace Application.Repository
{
    public interface IRepository<T>
    {
        Task<IQueryable> GetAsync(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
