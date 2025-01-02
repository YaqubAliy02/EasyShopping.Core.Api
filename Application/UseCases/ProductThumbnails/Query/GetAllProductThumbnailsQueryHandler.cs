using Application.DTOs.ProductThumbnails;
using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.ProductThumbnails.Query
{
    public class GetAllProductThumbnailsQuery : IRequest<IActionResult> { }
    public class GetAllProductThumbnailsQueryHandler : IRequestHandler<GetAllProductThumbnailsQuery, IActionResult>
    {
        private readonly IProductThumbnailRepository productThumbnailRepository;
        private readonly IMapper mapper;

        public GetAllProductThumbnailsQueryHandler(IProductThumbnailRepository productThumbnailRepository, IMapper mapper)
        {
            this.productThumbnailRepository = productThumbnailRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAllProductThumbnailsQuery request, CancellationToken cancellationToken)
        {
            var productThumbnails =  await this.productThumbnailRepository.GetAsync(x => true);

            var productThumbnailLists = await productThumbnails.ToListAsync();

            var productThumbnailDTO = this.mapper.Map<List<GetAllProductThumbnailDTO>>(productThumbnailLists);

            return new OkObjectResult(productThumbnailDTO);   
        }
    }
}