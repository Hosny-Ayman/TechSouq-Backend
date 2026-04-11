using FluentValidation;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name should not be empty");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password should not be empty");
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("RoleId should be valid");
        }
    }
}