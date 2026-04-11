using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class OrderItemValidator: AbstractValidator<OrderItemDto>
    {
       public OrderItemValidator() 
        {

            RuleFor(x => x.Quantity)
                    .GreaterThanOrEqualTo(1).WithMessage("Quantity Most be 1 or More");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(1).WithMessage("UnitPrice Most be 1 or More");

        
        }

    }
}
