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

           

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("CategoryName should be Not Empty");

            RuleFor(x => x.BrandName)
                 .NotEmpty().WithMessage("BrandName should be Not Empty");
        }
    }
}