using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

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

            if (result > 0) return subComment;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var subComment = this.easyShoppingDbContext.SubComments.Find(id);

            if (subComment is not null)
                this.easyShoppingDbContext.SubComments.Remove(subComment);

            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if (result > 0) return true;

            return false;

        }

        public async Task<IQueryable<SubComment>> GetAsync(Expression<Func<SubComment, bool>> expression)
        {
            return this.easyShoppingDbContext.SubComments
                 .Where(expression)
                 .AsQueryable();
        }

        public Task<SubComment> GetByIdAsync(Guid id)
        {
            return this.easyShoppingDbContext.SubComments
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<SubComment> UpdateAsync(SubComment subComment)
        {
            var subCommentResult = await this.GetByIdAsync(subComment.Id);
            if(subCommentResult is not null)
            {
                subCommentResult.Text = subComment.Text;
                int result = await this.easyShoppingDbContext.SaveChangesAsync();

                if (result > 0) return subCommentResult;
            }

            return null;


        }
    }
}
