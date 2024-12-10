using Application.Abstraction;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Categories.Query
{
    public class GetAllCategoriesQuery : IRequest<IActionResult> { }

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IActionResult>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public GetAllCategoriesQueryHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper
           )
        {
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            Task<IQueryable<Category>> categories = this.categoryRepository.GetAsync(x => true);

            var resultCategories = this.mapper
                .Map<IEnumerable<Category>>(categories.Result.AsEnumerable());

            return new OkObjectResult(resultCategories);
        }
    }
}
