using FluentValidation;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name should not be empty");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description should not be empty");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price should be greater than 0");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId should be valid");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId should be valid");

            RuleFor(x => x.BrandId)
                .GreaterThan(0).WithMessage("BrandId should be valid");
        }
    }
}