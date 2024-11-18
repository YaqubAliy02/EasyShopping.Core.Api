using Domain.Models;
using FluentValidation;

namespace Application.Validation
{
    public class CategoryValidation : AbstractValidator<Category>
    {
        public CategoryValidation()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Category name is required!");
        }
    }
}
