using System.Linq.Expressions;
using System.Security.Claims;
using Application.Models;
using Domain.Models;

namespace Application.Abstraction
{
    public interface ITokenService
    {
        public Task<Token> CreateTokenAsync(User user);
        public Task<Token> CreateTokenFromRefresh(ClaimsPrincipal principal, RefreshToken savedRefreshToken);
        public ClaimsPrincipal GetClaimsFromExpiredToken(string expiredToken);

        public Task<bool> AddRefreshToken(RefreshToken refreshToken);
        public bool Update(RefreshToken savedRefreshToken);
        public IQueryable<RefreshToken> Get(Expression<Func<RefreshToken, bool>> expression);
    }
}
