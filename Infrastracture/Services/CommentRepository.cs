﻿using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Services
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IEasyShoppingDbContext easyShoppingDbContext;

        public CommentRepository(IEasyShoppingDbContext easyShoppingDbContext)
        {
            this.easyShoppingDbContext = easyShoppingDbContext;
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            await this.easyShoppingDbContext.Comments.AddAsync(comment);
            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if (result > 0) return comment;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var comment = await this.easyShoppingDbContext.Comments.FindAsync(id);
            if (comment is not null)
                this.easyShoppingDbContext.Comments.Remove(comment);

            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if (result > 0) return true;

            return false;
        }

        public async Task<IQueryable<Comment>> GetAsync(Expression<Func<Comment, bool>> expression)
        {
            return this.easyShoppingDbContext.Comments
                 .Where(expression)
                 .Include(s => s.SubComments)
                 .AsSplitQuery()
                 .AsQueryable();
        }

        public Task<Comment> GetByIdAsync(Guid id)
        {
            return this.easyShoppingDbContext.Comments
                .Include(s => s.SubComments)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            var existingComment = await GetByIdAsync(comment.Id);
            if(existingComment is not null)
            {
                this.easyShoppingDbContext.Comments.Update(comment);
                var result =  await this.easyShoppingDbContext.SaveChangesAsync();

                if(result > 0) return comment;
            }

            return null;
        }
    }
}
