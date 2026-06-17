using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class DeliveryZoneValidator :AbstractValidator<DeliveryZoneDto>
    {

        public DeliveryZoneValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name Should Not Empty");

            RuleFor(x => x.ShippingCost)
              .GreaterThanOrEqualTo(1).WithMessage("ShippingCost Should Not be under 1");
        }

    }
}
