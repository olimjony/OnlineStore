using FluentValidation;
using OnlineStore.Application.DTOs;
namespace OnlineStore.Application.Validators;

public class UserRegisterValidator : AbstractValidator<UserRegisterDTO>
{
    public UserRegisterValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long");
    }
}
