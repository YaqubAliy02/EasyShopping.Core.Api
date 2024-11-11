using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
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
            IEasyShoppingDbContext context,
            int refreshTokenLifetime,
            int accessTokenLifetime)
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
            };

            List<string> roles = new();
            foreach (var role in user.UserRoles)
            {
                var eachRole = this.context.Roles
                    .Where(x => x.RoleId.Equals(role.RoleId))
                    .SingleOrDefault();

                if (eachRole is not null
                    && !roles.Contains(eachRole.Role))
                {
                    roles.Add(eachRole.Role);
                    claims.Add(new Claim(ClaimTypes.Role, eachRole.Role));
                }
            }
            claims = claims.Distinct().ToList();
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
            await this.context.RefreshTokens.AddAsync(refreshToken);
            await this.context.SaveChangesAsync();

            return true;
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
               (DateTime.UtcNow.ToString() + this.configuration["JWT:Key"]).GetHash();

    }
}
