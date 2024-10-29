using Domain.Models;
using FluentValidation;

namespace Application.Validation
{
    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(user => user.UserName).NotEmpty().WithMessage("UserName is required!");
            RuleFor(user => user.UserRoles).NotEmpty().WithMessage($"{nameof(User.UserRoles)}");
            RuleFor(user => user.Email).NotEmpty().EmailAddress().WithMessage("Please provide a valid email address in the format: 'example@example.com'. ");
        }
    }
}
