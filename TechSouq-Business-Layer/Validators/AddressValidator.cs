using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using FluentValidation;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{

   

    public class AddressValidator: AbstractValidator<AddressDto>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street Should Not Empty");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City Should Not Empty");

            RuleFor(x => x.Country)
              .NotEmpty().WithMessage("country Should Not Empty");

            RuleFor(x => x.FirstName)
               .NotEmpty().WithMessage("FirstName Should Not Empty");

            RuleFor(x => x.LastName)
               .NotEmpty().WithMessage("LastName Should Not Empty");

            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId is required");

            RuleFor(x => x.Phone)
                .NotNull().MinimumLength(11).MaximumLength(11).WithMessage("Phone Must be Not Null Or Less Or More Then 11 Number");
        }
        



    }
}
