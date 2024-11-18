using Application.Models;
using Application.Repository;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Categories.Command
{
    public class CreateCategoryCommand : IRequest<ResponseCore<CreateCategoryCommandHandlerResult>>
    {
        public string Name { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ResponseCore<CreateCategoryCommandHandlerResult>>
    {
        private readonly IMapper mapper;
        private readonly IValidator<Category> validator;
        private readonly ICategoryRepository categoryRepository;

        public CreateCategoryCommandHandler(
            IMapper mapper,
            IValidator<Category> validator,
            ICategoryRepository categoryRepository)
        {
            this.mapper = mapper;
            this.validator = validator;
            this.categoryRepository = categoryRepository;
        }

        public async Task<ResponseCore<CreateCategoryCommandHandlerResult>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<CreateCategoryCommandHandlerResult>();

            Category category = this.mapper.Map<Category>(request);
            var validationResult = this.validator.Validate(category);

            if(!validationResult.IsValid)
            {
                result.ErrorMessage = validationResult.Errors.ToArray();
                result.StatusCode = 400;

                return result;
            }

            if(category is null)
            {
                result.ErrorMessage = new string[] { "Category is not found" };
                result.StatusCode = 404;

                return result;
            }

            category = await this.categoryRepository.AddAsync(category);

            result.Result = this.mapper.Map<CreateCategoryCommandHandlerResult>(category);
            result.StatusCode = 200;

            return result;
        }
    }

    public class CreateCategoryCommandHandlerResult
    {
        public string Name { get; set; }
    }
}
