using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class OrderValidator : AbstractValidator<OrderDto>
    {

        public OrderValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status should not be empty");

            RuleFor(x => x.ShippingStreet)
                .NotEmpty()
                .WithMessage("ShippingStreet should not be empty");

            RuleFor(x => x.ShippingCity)
                .NotEmpty()
                .WithMessage("ShippingCity should not be empty");

            RuleFor(x => x.TotalAmount)
              .GreaterThan(0)
              .WithMessage("TotalAmount should not be 0 or les");

            RuleFor(x => x.UserId)
              .GreaterThan(0)
              .WithMessage("UserId should not be 0 or les");

            RuleFor(x => x.DeliveryMethodId)
              .GreaterThan(0)
              .WithMessage("TotalAmount should not be 0 or les");

            RuleFor(x => x.AddressId)
              .GreaterThan(0)
              .WithMessage("TotalAmount should not be 0 or les");
        }

    }
}
