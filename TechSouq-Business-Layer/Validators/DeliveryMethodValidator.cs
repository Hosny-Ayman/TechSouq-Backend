using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class DeliveryMethodValidator:AbstractValidator<DeliveryMethodDto>
    {

        public DeliveryMethodValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Cost)
               .GreaterThan(0).WithMessage("Cost must be greater than 0");


        }

    }
}
