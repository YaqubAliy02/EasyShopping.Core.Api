using Domain.Models;

namespace Application.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdatePasswordAsync(User user);
    }
}
