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
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserRepository userRepository;

        public UpdateProductCommandHandler(
            IMapper mapper,
            IProductRepository productRepository,
            IValidator<Product> validator,
            IHttpContextAccessor httpContextAccessor,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.validator = validator;
            this.httpContextAccessor = httpContextAccessor;
            this.categoryRepository = categoryRepository;
            this.userRepository = userRepository;
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

            var duplicateProduct = await productRepository.GetAsync(p => p.Name == request.Name && p.Id != request.Id);
            if (duplicateProduct.Any())
                return new BadRequestObjectResult("A product with the same name already exists");

            if (request.SellingPrice < request.CostPrice)
                return new BadRequestObjectResult("Selling price cannot be lower than cost price");

            if (request.StockQuantity < 0)
                return new BadRequestObjectResult("Stock quantity cannot be negative");

            var category = await categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
                return new BadRequestObjectResult("Invalid category");

            var user = await userRepository.GetByIdAsync(Guid.Parse(userId));
            if (user == null)
                return new BadRequestObjectResult("Invalid user");

            var product = mapper.Map(request, existingProduct);
            product.UpdatedAt = DateTimeOffset.UtcNow;

            var validationResult = validator.Validate(product);
            if (!validationResult.IsValid)
                return new BadRequestObjectResult(validationResult);

            product = await productRepository.UpdateAsync(product);
            var productGetDto = mapper.Map<ProductGetDto>(product);

            return new OkObjectResult(productGetDto);
        }
    }
}
