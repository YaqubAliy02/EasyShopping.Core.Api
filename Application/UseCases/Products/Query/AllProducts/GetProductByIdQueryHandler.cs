using Application.DTOs.Products;
using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Products.Query.AllProducts
{
    public class GetProductByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, IActionResult>
    {
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;

        public GetProductByIdQueryHandler(IMapper mapper, IProductRepository productRepository)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await this.productRepository.GetByIdAsync(request.Id);

            if(product is null)
            {
                return new NotFoundObjectResult($"Product id: {request.Id} is not found");
            }

            var productDto = this.mapper.Map<ProductGetDto>(product);

            return new OkObjectResult(productDto);
        }
    }
}
