using FluentValidation;
using OnlineStore.Application.DTOs;

namespace OnlineStore.Application.Validators;

public class CreateCartValidator : AbstractValidator<CreateCartDTO>
{
    public CreateCartValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("The length should be less than 50 symbols!")
            .NotEmpty().WithMessage("Name of the Product is required");
    }
}
