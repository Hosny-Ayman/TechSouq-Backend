using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class CartItemValidator : AbstractValidator<CartItemDto>
    {
        public CartItemValidator() 
        {
          

            RuleFor(x=>x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.");
        }


    }
}
