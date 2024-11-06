using Domain.Models;
using FluentValidation;

namespace Application.Validation
{
    public class UserRoleValidation : AbstractValidator<UserRole>
    {
        public UserRoleValidation()
        {
            RuleFor(role => role.Role).NotEmpty().WithMessage("Role is required"); ;
        }
    }
}
