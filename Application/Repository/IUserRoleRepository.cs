using Domain.Models;

namespace Application.Repository
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<UserRole> GetRoleByNameAsync(string roleName);
    }
}
