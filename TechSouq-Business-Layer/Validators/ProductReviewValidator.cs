using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class ProductReviewValidator:AbstractValidator<ProductReviewDto>
    {
        public ProductReviewValidator()
        {
            RuleFor(x => x.ProductId)
           .GreaterThan(0)
           .WithMessage("ProductId must be greater than 0");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("UserId must be greater than 0");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required")
                .MaximumLength(500).WithMessage("Comment must not exceed 500 characters");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5");
        }


    }
}
