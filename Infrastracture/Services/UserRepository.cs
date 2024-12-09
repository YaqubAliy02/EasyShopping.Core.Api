using System.Linq.Expressions;
using Application.Abstraction;
using Application.DTOs.Users;
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

        public async Task<List<User>> GetAsync(Expression<Func<User, bool>> expression)
        {
            return await this.easyShoppingDbContext.Users.Where(expression)
                .Include(x => x.Products)
                .Include(x => x.Comments)
                .Include(x => x.ShoppingCart)
                .Include(x => x.UserRoles)
                .Include(x => x.Orders).ToListAsync();
        }

        public async Task<UserGetAllDto> GetByIdAsync(Guid id)
        {
            return await this.easyShoppingDbContext.Users
                .Where(x => x.Id == id)
                .Select(user => new UserGetAllDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = user.Password,
                    ProductsId = user.Products.Select(p => p.Id).ToList()
                    
                })
                .FirstOrDefaultAsync();
        }


        public async Task<User> UpdateAsync(UserGetAllDto userDto)
        {
            var existingUser = await this.easyShoppingDbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userDto.Id);

            if (existingUser is not null)
            {
                existingUser.UserName = userDto.UserName;
                existingUser.Email = userDto.Email;

                existingUser.UserRoles.Clear();

                foreach (var roleDto in userDto.RolesId)
                {
                    var role = await this.easyShoppingDbContext.Roles.FindAsync(roleDto);
                    if (role is not null)
                    {
                        existingUser.UserRoles.Add(role);
                    }
                }

                await this.easyShoppingDbContext.SaveChangesAsync();
                return existingUser;
            }

            return null; 
        }

        public Task<User> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatePasswordAsync(User user)
        {
            var existingUser = await GetByIdAsync(user.Id);

            if (existingUser is not null)
            {
                existingUser.Password = user.Password;
                await this.easyShoppingDbContext.SaveChangesAsync();
            }
        }

        Task<User> IRepository<User>.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

