using System.Security.Claims;
using Application.Abstraction;
using Application.DTOs.Products;
using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Products.Query.OwnProduct
{
    public class GetAllOwnProductsQuery : IRequest<IActionResult> { }

    public class GetAllOwnProductsQueryHandler : IRequestHandler<GetAllOwnProductsQuery, IActionResult>
    {
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IProductRepository productRepository;

        public GetAllOwnProductsQueryHandler(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IProductRepository productRepository)

        {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Handle(GetAllOwnProductsQuery request, CancellationToken cancellationToken)
        {
            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return new UnauthorizedResult();
            }

            var userIdClaim = httpContextAccessor
                 .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = Guid.Parse(userIdClaim);

            var products = await productRepository.GetAsync(x => x.UserId == userId);
            var productList = await products.ToListAsync();

            var productsDto = mapper.Map<List<ProductGetDto>>(products);

            return new OkObjectResult(productsDto);
        }
    }
}