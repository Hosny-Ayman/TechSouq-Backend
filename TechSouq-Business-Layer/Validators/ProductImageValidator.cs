using FluentValidation;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class ProductImageValidator : AbstractValidator<ProductImageDto>
    {
        public ProductImageValidator()
        {
            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("ImageUrl should not be empty");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId should be valid");
        }
    }
}