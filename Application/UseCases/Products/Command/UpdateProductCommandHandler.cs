using System.Security.Claims;
using Application.DTOs.Products;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Products.Command
{
    public class UpdateProductCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public string Thumbnail { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, IActionResult>
    {
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;
        private readonly IValidator<Product> validator;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UpdateProductCommandHandler(
            IMapper mapper,
            IProductRepository productRepository,
            IValidator<Product> validator,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.validator = validator;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var userId = httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return new UnauthorizedObjectResult("User is not authenticated");

            var existingProduct = await productRepository.GetByIdAsync(request.Id);
            if (existingProduct == null)
                return new NotFoundObjectResult("Product not found");

            if (existingProduct.UserId.ToString() != userId)
                return new ForbidResult("User is not authorized to update this product");

            mapper.Map(request, existingProduct);
            existingProduct.UpdatedAt = DateTimeOffset.UtcNow;

            var validationResult = validator.Validate(existingProduct);

            if (!validationResult.IsValid)
                return new BadRequestObjectResult(validationResult);

            var updatedProduct = await productRepository.UpdateAsync(existingProduct);
            var productGetDto = mapper.Map<ProductGetDto>(updatedProduct);

            return new OkObjectResult(productGetDto);
        }
    }
}
