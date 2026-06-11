using FluentValidation;
using TechSouq.Application.Dtos;

namespace TechSouq.Application.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email should not be empty.")
                .EmailAddress().WithMessage("A valid Email is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password should not be empty.");
                //.MinimumLength(8).WithMessage("OldPassword should not be less than 8 characters.")
                //.MaximumLength(30).WithMessage("OldPassword should not be more than 30 characters.");
        }
    }
}