﻿using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Services
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IEasyShoppingDbContext easyShoppingDbContext;

        public UserRoleRepository(IEasyShoppingDbContext easyShoppingDbContext)
        {
            this.easyShoppingDbContext = easyShoppingDbContext;
        }

        public async Task<UserRole> AddAsync(UserRole userRole)
        {
            this.easyShoppingDbContext.Roles.Add(userRole);
            int result = await this.easyShoppingDbContext.SaveChangesAsync();

            if (result > 0) return userRole;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            UserRole userRole = await this.easyShoppingDbContext.Roles.FindAsync(id);

            if (userRole is not null)
            {
                this.easyShoppingDbContext.Roles.Remove(userRole);
                int result = await this.easyShoppingDbContext.SaveChangesAsync();
                if (result > 0) return true;
            }

            return false;
        }

        public async Task<IQueryable<UserRole>> GetAsync(Expression<Func<UserRole, bool>> expression)
        {
            return this.easyShoppingDbContext
                .Roles.Where(expression).AsQueryable();
        }

        public async Task<UserRole> GetByIdAsync(Guid id)
        {
            return await this.easyShoppingDbContext
                .Roles.Where(x => x.RoleId == id).SingleOrDefaultAsync();
        }

        public async Task<UserRole> GetRoleByNameAsync(string roleName)
        {
            return await this.easyShoppingDbContext
                .Roles.Where(x => x.Role == roleName).FirstOrDefaultAsync();
        }

        public async Task<UserRole> UpdateAsync(UserRole updatedRole)
        {
            var existingRole = await GetByIdAsync(updatedRole.RoleId);

            if (existingRole is not null)
            {
                existingRole.Role = updatedRole.Role;
                int result = await this.easyShoppingDbContext.SaveChangesAsync();

                if (result > 0) return updatedRole;
            }

            return null;
        }
    }
}
