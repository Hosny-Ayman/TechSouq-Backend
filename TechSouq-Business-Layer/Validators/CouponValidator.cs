using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class CouponValidator : AbstractValidator<CouponDto>
    {

        public CouponValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty();

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0);

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.UtcNow);

            RuleFor(x => x.UsageLimit)
                .GreaterThanOrEqualTo(0);
        }
    }
}
