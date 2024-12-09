using System.Security.Claims;
using Application.Abstraction;
using Application.DTOs.Products;
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
        private readonly IEasyShoppingDbContext easyShoppingDbContext;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetAllProductsQueryHandler(
            IEasyShoppingDbContext easyShoppingDbContext,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)

        {
            this.easyShoppingDbContext = easyShoppingDbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var userIdClaim = this.httpContextAccessor
                 .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = Guid.Parse(userIdClaim);

            var products = await this.easyShoppingDbContext.Products
                .Where(p => p.UserId == userId)
                .ToListAsync(cancellationToken);

            var productsDto = this.mapper.Map<List<ProductGetDto>>(products);

            return new OkObjectResult(productsDto);
            
        }
    }
}
// var productsDto = this.mapper.Map<List<ProductGetDto>>(products); I have to return this productDto
// it is not working and error message: Object reference not set to an instance of an object. actually products is not null
// that's why mapper just map Product to ProductGetDto but it is not.