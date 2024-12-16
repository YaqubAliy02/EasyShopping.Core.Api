using System.Security.Claims;
using Application.DTOs.Products;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Products.Query
{
    public class GetProductByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, IActionResult>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetProductByIdQueryHandler(
            IProductRepository productRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            if (!this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return new UnauthorizedResult();
            }
            var userIdClaim = this.httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = Guid.Parse(userIdClaim);

            Product product = await this.productRepository.GetByIdAsync(request.Id);

            if (product is null || product.UserId != userId)
                return new NotFoundObjectResult($"Product id: {request.Id} is not found");

            var productDto = this.mapper.Map<ProductGetDto>(product);

            return new OkObjectResult(productDto);
        }
    }
}
