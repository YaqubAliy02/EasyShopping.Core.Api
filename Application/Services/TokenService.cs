using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstraction;
using Application.Extensions;
using Application.Models;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly IEasyShoppingDbContext context;
        private readonly int refreshTokenLifetime;
        private readonly int accessTokenLifetime;

        public TokenService(
            IConfiguration configuration,
            IEasyShoppingDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
            this.refreshTokenLifetime = int.Parse(configuration["JWT:RefreshTokenLifetime"]);
            this.accessTokenLifetime = int.Parse(configuration["JWT:AccessTokenLifetime"]);
        }

        public async Task<Token> CreateTokenAsync(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var userRoleIds = user.UserRoles.Select(r => r.RoleId).ToList();
            var roles = this.context.Roles
                .Where(r => userRoleIds.Contains(r.RoleId))
                .Select(r => r.Role).ToList();
            claims = claims.Distinct().ToList();

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            Token tokens = CreateToken(claims);

            var savedRefreshToken = Get(x => x.Email == user.Email).FirstOrDefault();
            if (savedRefreshToken is null)
            {
                var refreshToken = new RefreshToken()
                {
                    ExpiredDate = DateTime.UtcNow.AddMinutes(this.refreshTokenLifetime),
                    RefreshTokenValue = tokens.RefreshToken,
                    Email = user.Email,
                };
                await AddRefreshToken(refreshToken);
            }
            else
            {
                savedRefreshToken.RefreshTokenValue = tokens.RefreshToken;
                savedRefreshToken.ExpiredDate = DateTime.UtcNow.AddMinutes(this.refreshTokenLifetime);
                Update(savedRefreshToken);
            }

            return tokens;
        }
        public Token CreateToken(IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: this.configuration["JWT:IssuerKey"],
                audience: this.configuration["JWT:AudienceKey"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(this.configuration["JWT:AccessTokenLifetime"])),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(this.configuration["JWT:Key"])),
                        SecurityAlgorithms.HmacSha256Signature));

            Token tokens = new Token()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = GenerateRefreshToken()
            };
            return tokens;
        }

        public async Task<bool> AddRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                await this.context.RefreshTokens.AddAsync(refreshToken);
                await this.context.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    "Failed to add refresh token" + exception.Message);
            }
        }

        public bool Update(RefreshToken savedRefreshToken)
        {
            this.context.RefreshTokens.Update(savedRefreshToken);
            this.context.SaveChangesAsync();

            return true;
        }

        public IQueryable<RefreshToken> Get(Expression<Func<RefreshToken, bool>> expression) =>
               this.context.RefreshTokens.Where(expression);


        private string GenerateRefreshToken() =>
              Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        public Task<Token> CreateTokenFromRefresh(ClaimsPrincipal principal, RefreshToken savedRefreshToken)
        {
            Token tokens = CreateToken(principal.Claims);
            savedRefreshToken.RefreshTokenValue = tokens.RefreshToken;
            savedRefreshToken.ExpiredDate = DateTime.UtcNow.AddMinutes(this.refreshTokenLifetime);

            Update(savedRefreshToken);

            return Task.FromResult(tokens);
        }

        public ClaimsPrincipal GetClaimsFromExpiredToken(string expiredToken)
        {
            byte[] key = Encoding.UTF8.GetBytes(this.configuration["JWT:Key"]);

            var tokenParams = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = this.configuration["JWT:AudienceKey"],
                ValidateIssuer = true,
                ValidateLifetime = false,
                ValidIssuer = this.configuration["JWT:IssuerKey"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = tokenHandler
                .ValidateToken(expiredToken, tokenParams, out SecurityToken securityToken);

            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken is null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
