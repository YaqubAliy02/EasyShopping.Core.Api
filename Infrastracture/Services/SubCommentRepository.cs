using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repository;
using Domain.Models;

namespace Infrastracture.Services
{
    public class SubCommentRepository : ISubCommentRepository
    {
        private readonly IEasyShoppingDbContext easyShoppingDbContext;

        public SubCommentRepository(IEasyShoppingDbContext easyShoppingDbContext)
        {
            this.easyShoppingDbContext = easyShoppingDbContext;
        }

        public async Task<SubComment> AddAsync(SubComment subComment)
        {
            await this.easyShoppingDbContext.SubComments.AddAsync(subComment);
            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if(result > 0) return subComment;

            return null;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<SubComment>> GetAsync(Expression<Func<SubComment, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<SubComment> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<SubComment> UpdateAsync(SubComment entity)
        {
            throw new NotImplementedException();
        }
    }
}
