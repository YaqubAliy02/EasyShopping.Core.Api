using Application.DTOs.Products;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
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

        public GetProductByIdQueryHandler(
            IProductRepository productRepository,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            Product product = await this.productRepository.GetByIdAsync(request.Id);

            var productDto = this.mapper.Map<ProductGetDto>(product);  

            if (productDto is null)
            {
                return new NotFoundObjectResult($"Product id: {request.Id} is not found");
            }

            return new OkObjectResult(productDto);
        }
    }
}
