using Application.UseCases.Products.Command;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.Validation
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CostPrice).GreaterThan(0);
            RuleFor(x => x.SellingPrice).GreaterThan(0);
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
