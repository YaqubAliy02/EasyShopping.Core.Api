using System.Linq.Expressions;
using Application.Abstraction;
using Application.Extensions;
using Application.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Services
{
    internal class UserRepository : IUserRepository
    {
        private readonly IEasyShoppingDbContext easyShoppingDbContext;

        public UserRepository(IEasyShoppingDbContext easyShoppingDbContext)
        {
            this.easyShoppingDbContext = easyShoppingDbContext;
        }

        public async Task<User> AddAsync(User user)
        {
            user.Password = user.Password.GetHash();
            this.easyShoppingDbContext.Users.Add(user);

            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if (result > 0) return user;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            User user = await this.easyShoppingDbContext.Users.FindAsync(id);

            if (user is not null)
            {
                this.easyShoppingDbContext.Users.Remove(user);
            }

            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if (result > 0) return true;

            return false;
        }

        public async Task<IQueryable<User>> GetAsync(Expression<Func<User, bool>> expression)
        {
            return this.easyShoppingDbContext.Users.Where(expression)
                .Include(x => x.Products)
                .Include(x => x.Comments)
                .Include(x => x.ShoppingCart)
                .Include(x => x.SubComments)
                .Include(x => x.UserRoles)
                .Include(x => x.Orders);
        }

        public Task<User> GetByIdAsync(Guid id)
        {
            return easyShoppingDbContext.Users
                .Where(x => x.Id.Equals(id))
                .Include(x => x.Products)
                .Include(x => x.Comments)
                .Include(x => x.ShoppingCart)
                .Include(x => x.SubComments)
                .Include(x => x.UserRoles)
                .Include(x => x.Orders)
                .FirstOrDefaultAsync();
        }

        public async Task<User> UpdateAsync(User user)
        {
            var existingUser = await GetByIdAsync(user.Id);

            if (existingUser is not null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;

                existingUser.UserRoles.Clear();

                foreach (var role in user.UserRoles)
                {
                    var existingRole = await this.easyShoppingDbContext.Roles.FindAsync(role.RoleId);

                    if (existingRole is not null)
                    {
                        existingUser.UserRoles.Add(existingRole);
                    }
                }

                int result = await this.easyShoppingDbContext.SaveChangesAsync();

                if (result > 0) return user;
            }

            return null;
        }

    }
}

