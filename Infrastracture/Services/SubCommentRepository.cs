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

        public async Task<bool> DeleteAsync(Guid id)
        {
            var subComment = this.easyShoppingDbContext.SubComments.Find(id);

            if(subComment is not null)
                this.easyShoppingDbContext.SubComments.Remove(subComment);

            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if(result > 0) return true;

            return false;

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
