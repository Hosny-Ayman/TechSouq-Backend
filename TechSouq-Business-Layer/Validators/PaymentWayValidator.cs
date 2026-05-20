using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class PaymentWayValidator :AbstractValidator<PaymentWayDto>
    {
        public PaymentWayValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("PaymentWay Name Is Required");
        }
    }
}
