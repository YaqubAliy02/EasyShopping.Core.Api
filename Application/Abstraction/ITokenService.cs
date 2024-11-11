using System.Linq.Expressions;
using Application.Models;
using Domain.Models;

namespace Application.Abstraction
{
    public interface ITokenService
    {
        public Task<Token> CreateTokenAsync(User user);
        public Task<bool> AddRefreshToken(RefreshToken refreshToken);
        public bool Update(RefreshToken savedRefreshToken);
        public IQueryable<RefreshToken> Get(Expression<Func<RefreshToken, bool>> expression);
    }
}
