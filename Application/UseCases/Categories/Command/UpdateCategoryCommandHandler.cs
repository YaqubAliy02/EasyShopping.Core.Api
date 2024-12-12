using Application.Repository;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Categories.Command
{
    public class UpdateCategoryCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, IActionResult>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public UpdateCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = this.mapper.Map<Category>(request);
            category = await this.categoryRepository.UpdateAsync(category);

            if (category is null)
                return new NotFoundObjectResult("Category is not found to complete update operation!");

            return new OkObjectResult(category);
        }
    }
}
