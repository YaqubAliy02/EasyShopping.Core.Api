using System.Security.Claims;
using Application.DTOs.Products;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Products.Query.OwnProduct
{
    public class GetOwnProductByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class GetOwnProductByIdQueryHandler : IRequestHandler<GetOwnProductByIdQuery, IActionResult>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetOwnProductByIdQueryHandler(
            IProductRepository productRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Handle(GetOwnProductByIdQuery request, CancellationToken cancellationToken)
        {
            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return new UnauthorizedResult();
            }
            var userIdClaim = httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = Guid.Parse(userIdClaim);

            Product product = await productRepository.GetByIdAsync(request.Id);

            if (product is null || product.UserId != userId)
                return new NotFoundObjectResult($"Product id: {request.Id} is not found");

            var productDto = mapper.Map<ProductGetDto>(product);

            return new OkObjectResult(productDto);
        }
    }
}
