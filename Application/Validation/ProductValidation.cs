using Application.UseCases.Products.Command;
using Domain.Models;
using FluentValidation;

namespace Application.Validation
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required!");
            RuleFor(x => x.CostPrice).GreaterThan(0);
            RuleFor(x => x.SellingPrice).GreaterThan(0);
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
