using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class SystemSettingValidator: AbstractValidator<SystemSettingDto>
    {
        public SystemSettingValidator()
        {
            RuleFor(x => x.SettingKey)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.SettingValue)
                .NotEmpty();

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }

    }
}
