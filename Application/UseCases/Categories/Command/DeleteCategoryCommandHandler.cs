using Application.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Categories.Command
{
    public class DeleteCategoryCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, IActionResult>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public DeleteCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            bool isDelete = await this.categoryRepository.DeleteAsync(request.Id);

            return isDelete ? new OkObjectResult("Category is deleted successfully") :
                new NotFoundObjectResult("Category is not found to complete delete operation!");
        }
    }
}
