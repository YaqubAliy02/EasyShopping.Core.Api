using System.Security.Claims;
using System.Text.Json.Serialization;
using Application.Abstraction;
using Application.Models;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Accounts.Command
{
    public class RefreshTokenCommand : IRequest<ResponseCore<RefreshTokenCommandResult>>
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResponseCore<RefreshTokenCommandResult>>
    {
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public RefreshTokenCommandHandler(
            ITokenService tokenService,
            IMapper mapper)
        {
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<ResponseCore<RefreshTokenCommandResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<RefreshTokenCommandResult>();

            var principal = this.tokenService.GetClaimsFromExpiredToken(request.AccessToken);
            string email = principal.FindFirstValue(ClaimTypes.Email);

            if (email is null)
            {
                result.ErrorMessage = new string[] { "Refresh token is not found" };
                result.StatusCode = 420;

                return result;
            }

            RefreshToken savedRefreshToken = await this.tokenService.Get(x =>
                x.Email == email && x.RefreshTokenValue == request.RefreshToken)
                    .AsNoTracking().FirstOrDefaultAsync();

            if (savedRefreshToken is null)
            {
                result.ErrorMessage = new string[] { "Refresh token or access token is invalid" };
                result.StatusCode = 405;

                return result;
            }

            Token newTokens = await this.tokenService.CreateTokenFromRefresh(principal, savedRefreshToken);

            result.Result = this.mapper.Map<RefreshTokenCommandResult>(newTokens);
            result.StatusCode = 200;

            return result;
        }
    }

    public class RefreshTokenCommandResult
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
