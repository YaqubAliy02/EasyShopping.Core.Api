using Domain.Models;
using FluentValidation;

namespace Application.Validation
{
    public class CommentValidation : AbstractValidator<Comment>
    {
        public CommentValidation()
        {
            RuleFor(c => c.Text).NotEmpty().WithMessage("Comment text is required.");
        }
    }
}
