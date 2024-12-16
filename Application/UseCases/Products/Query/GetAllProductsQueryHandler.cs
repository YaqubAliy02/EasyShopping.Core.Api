using System.Security.Claims;
using Application.Abstraction;
using Application.DTOs.Products;
using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Products.Query
{
    public class GetAllProductsQuery : IRequest<IActionResult> { }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IActionResult>
    {
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IProductRepository productRepository;

        public GetAllProductsQueryHandler(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IProductRepository productRepository)

        {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            if (!this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return new UnauthorizedResult();
            }

            var userIdClaim = this.httpContextAccessor
                 .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = Guid.Parse(userIdClaim);

            var products = await this.productRepository.GetAsync(x => x.UserId == userId);
            var productList = await products.ToListAsync();

            var productsDto = this.mapper.Map<List<ProductGetDto>>(products);

            return new OkObjectResult(productsDto);
        }
    }
}