using FluentValidation;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class RoleValidator : AbstractValidator<RoleDto>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name should not be empty");
        }
    }
}