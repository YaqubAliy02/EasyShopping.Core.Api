using Application.DTOs.ProductThumbnails;
using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.ProductThumbnails.Query
{
    public class GetProductThumbnailByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class GetProductThumbnailByIdQueryHandler : IRequestHandler<GetProductThumbnailByIdQuery, IActionResult>
    {
        private readonly IProductThumbnailRepository productThumbnailRepository;
        private readonly IMapper mapper;

        public GetProductThumbnailByIdQueryHandler(
            IProductThumbnailRepository productThumbnailRepository,
            IMapper mapper)
        {
            this.productThumbnailRepository = productThumbnailRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetProductThumbnailByIdQuery request, CancellationToken cancellationToken)
        {
            var productThumbnail = await this.productThumbnailRepository.GetByIdAsync(request.Id);

            if(productThumbnail is null)
                return new BadRequestObjectResult(request);

            var productThumbnailGetDto = this.mapper.Map<GetAllProductThumbnailDTO>(productThumbnail);

            return new OkObjectResult(productThumbnailGetDto);
        }
    }
}
