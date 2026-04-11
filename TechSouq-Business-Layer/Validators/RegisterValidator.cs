using FluentValidation;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name should not be empty.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email should not be empty.")
                .EmailAddress().WithMessage("A valid Email is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password should not be empty.")
                .MinimumLength(8).WithMessage("Password should not be less than 8 characters.")
                .MaximumLength(30).WithMessage("Password should not be more than 30 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character (e.g. @, #, $, %).");

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("RoleId should be valid.");
        }
    }
}