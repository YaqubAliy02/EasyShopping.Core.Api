using Application.DTOs.Products;
using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Products.Query.AllProducts
{
    public class GetAllProductsQuery : IRequest<IActionResult> { }
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IActionResult>
    {
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;

        public GetAllProductsQueryHandler(IMapper mapper, IProductRepository productRepository)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await this.productRepository.GetAsync(x => true);

            var productList = await products.ToListAsync();

            var productsDto = this.mapper.Map<List<ProductGetDto>>(productList);

            return new OkObjectResult(productsDto);
        }
    }
}
