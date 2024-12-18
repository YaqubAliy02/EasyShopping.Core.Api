using Domain.Models;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.Validation
{
    public class SubCommentValidation : AbstractValidator<SubComment>
    {
        public SubCommentValidation()
        {
            RuleFor(s => s.Text).NotEmpty().WithMessage("Text is required");
        }
    }
}
