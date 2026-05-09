using FluentValidation;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Name should not be empty");
            RuleFor(x => x.SecondName).NotEmpty().WithMessage("Name should not be empty");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid Email is required");
            //RuleFor(x => x.OldPassword).NotEmpty().WithMessage("OldPassword should not be empty");
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("RoleId should be valid");
        }
    }
}