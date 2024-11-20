using System.Security.Claims;
using Application.Abstraction;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Products.Query
{
    public class GetAllProductsQuery : IRequest<IActionResult> { }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IActionResult>
    {
        private readonly IEasyShoppingDbContext easyShoppingDbContext;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetAllProductsQueryHandler(
            IEasyShoppingDbContext easyShoppingDbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            this.easyShoppingDbContext = easyShoppingDbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
        
            var userIdClaim = this.httpContextAccessor
                 .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = Guid.Parse(userIdClaim);

            var products = await this.easyShoppingDbContext.Products
                .Where(p => p.UserId == userId)
                .ToListAsync(cancellationToken);

            return new OkObjectResult(products);
        }
    }
}
