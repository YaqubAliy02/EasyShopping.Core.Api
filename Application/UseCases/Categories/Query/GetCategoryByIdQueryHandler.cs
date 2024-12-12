using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Categories.Query
{
    public class GetCategoryByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, IActionResult>
    {
        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;

        public GetCategoryByIdQueryHandler(
            IMapper mapper, 
            ICategoryRepository categoryRepository)
        {
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category =  await this.categoryRepository.GetByIdAsync(request.Id);

            if(category is null) 
                return new NotFoundObjectResult("Category is not not found");

            var result = this.mapper.Map<Category>(category);

            return new OkObjectResult(result);
        }
    }
}
